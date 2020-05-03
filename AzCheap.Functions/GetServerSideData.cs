using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.Text;

namespace AzCheap.Functions
{
    public static class GetServerSideData
    {
        [FunctionName("GetServerSideData")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
                                                          ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "{  \"draw\": 1,  \"recordsTotal\": 57,  \"recordsFiltered\": 57,  \"data\": [    [      \"Airi\",      \"Satou\",      \"Accountant\",      \"Tokyo\",      \"28th Nov 08\",      \"$162,700\"    ],    [      \"Angelica\",      \"Ramos\",      \"Chief Executive Officer (CEO)\",      \"London\",      \"9th Oct 09\",      \"$1,200,000\"    ],    [      \"Ashton\",      \"Cox\",      \"Junior Technical Author\",      \"San Francisco\",      \"12th Jan 09\",      \"$86,000\"    ],    [      \"Bradley\",      \"Greer\",      \"Software Engineer\",      \"London\",      \"13th Oct 12\",      \"$132,000\"    ],    [      \"Brenden\",      \"Wagner\",      \"Software Engineer\",      \"San Francisco\",      \"7th Jun 11\",      \"$206,850\"    ],    [      \"Brielle\",      \"Williamson\",      \"Integration Specialist\",      \"New York\",      \"2nd Dec 12\",      \"$372,000\"    ],    [      \"Bruno\",      \"Nash\",      \"Software Engineer\",      \"London\",      \"3rd May 11\",      \"$163,500\"    ],    [      \"Caesar\",      \"Vance\",      \"Pre-Sales Support\",      \"New York\",      \"12th Dec 11\",      \"$106,450\"    ],    [      \"Cara\",      \"Stevens\",      \"Sales Assistant\",      \"New York\",      \"6th Dec 11\",      \"$145,600\"    ],    [      \"Cedric\",      \"Kelly\",      \"Senior Javascript Developer\",      \"Edinburgh\",      \"29th Mar 12\",      \"$433,060\"    ]  ]}";

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseMessage, Encoding.UTF8, "application/json")
            };
        }
    }
}
