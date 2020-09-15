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
using Microsoft.OData.UriParser;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.IO;

namespace AzCheap.Functions
{
    public static class GetServerSideData
    {
        [FunctionName("GetServerSideData")]
        public async static Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
                                                          ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request." + Environment.GetEnvironmentVariable("LocalSettingValue"));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(requestBody);

            var cache = ConnectionMultiplexer.Connect("52.141.218.12").GetDatabase();
            var totalRecords = await cache.SortedSetLengthAsync("AccountData");

            // if the data has not been loaded into Redis
            if (totalRecords == 0)
            {
                // retrieve from table storage
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=cheapstorageus;AccountKey=2IaxLhniogDP0N2cEeT9Bc27rVvRsNrZUF7tTHRro8Qvp52mmwpPdhcnhlHhB/oTtDq6dux2Doj7gQEjdOTk5g==;TableEndpoint=https://cheapstorageus.table.core.windows.net/;");

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

                    //await cache.SetAddAsync("AccountData", items.Select(i => new RedisValue(JsonConvert.SerializeObject(i))).ToArray());                    
                    await cache.SortedSetAddAsync("AccountData", items.Select(i => new SortedSetEntry(JsonConvert.SerializeObject(i), int.Parse(i.RowKey))).ToArray());

                    totalRecords = await cache.SortedSetLengthAsync("AccountData");
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                }
            }

            
            var accountData = await cache.SortedSetRangeByScoreAsync("AccountData", model.start, model.start + model.length);

            var response = new 
            { 
                draw = model.draw, 
                recordsTotal = totalRecords, 
                recordsFiltered = totalRecords,
                data = accountData
            };

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
