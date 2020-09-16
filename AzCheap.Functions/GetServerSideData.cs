using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.Text;

using StackExchange.Redis;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace AzCheap.Functions
{
    public static class GetServerSideData
    {
        [FunctionName("GetServerSideData")]
        public async static Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(requestBody);

            var cache = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("RedisCache")).GetDatabase();
            var totalRecords = await cache.SortedSetLengthAsync("AccountData");

            // if the data has not been loaded into Redis
            if (totalRecords == 0)
            {
                log.LogInformation($"AccountData no loaded in cache. Retrieving records from storage.");
                // retrieve from table storage
                var account = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("TableStorageConnectionString"));

                var tableClient = account.CreateCloudTableClient();
                var table = tableClient.GetTableReference("AccountData");

                var items = new List<AccountData>();

                try
                {
                    var query = new TableQuery<AccountData>();
                    TableContinuationToken token = null;
                    do
                    {
                        var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                        token = resultSegment.ContinuationToken;

                        items.AddRange(resultSegment.Results);
                    } while (token != null);

                    log.LogInformation($"Saving AccountData to cache.");

                    await cache.SortedSetAddAsync("AccountData", items.Select(i => new SortedSetEntry(JsonConvert.SerializeObject(i), int.Parse(i.RowKey))).ToArray());

                    totalRecords = await cache.SortedSetLengthAsync("AccountData");
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                }
            }

            log.LogInformation($"Retrieving range from cache.");

            var accountData = await cache.SortedSetRangeByScoreAsync("AccountData", model.start, model.start + model.length);

            var response = new 
            { 
                draw = model.draw, 
                recordsTotal = totalRecords, 
                recordsFiltered = totalRecords,
                data = accountData
            };

            log.LogInformation($"Returning successful response for {accountData.Length} out of {totalRecords} records.");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
            };
        }
    }    

    public class AccountData : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Office { get; set; }
        public string Position { get; set; }
        public string Salary { get; set; }
        public string StartDate { get; set; }
    }

    public class DataTableAjaxPostModel
    {
        // properties are not capital due to json mapping
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
