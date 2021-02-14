using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TestFunction
{
    public static class SendMessage
    {
        [FunctionName("SendMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [EventHub(eventHubName: "waltereventhub", Connection = "EventHubConnectionString")] IAsyncCollector<string> outputEvents,
            ILogger log)
        {
            var message = await new StreamReader(req.Body).ReadToEndAsync();

            await outputEvents.AddAsync(message);

            return new OkObjectResult("Event Sent!");
        }
    }
}
