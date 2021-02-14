using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;

namespace TestFunction
{
    public class EventHubListener
    {
        [FunctionName(nameof(EventHubListenerTest))]
        public Task EventHubListenerTest(
            [EventHubTrigger("waltereventhub", Connection = "EventHubConnectionString")] Microsoft.Azure.EventHubs.EventData[] events,
            [ServiceBus("fromeventhub", Connection = "ServiceBusConnectionStringFromEventHub")] out Message message,
            ILogger log)
        {
            message = new Message(Encoding.ASCII.GetBytes(""));

            foreach (var e in events)
            {
                var payLoad = Encoding.UTF8.GetString(e.Body.Array, e.Body.Offset, e.Body.Count);
                log.LogWarning("Sending message to the Service Bus");

                message = new Message(Encoding.ASCII.GetBytes(payLoad));
                message.MessageId = CreateDeterministicIdFromHash(payLoad);
                message.CorrelationId = CreateDeterministicIdFromHash(payLoad);
            }

            return Task.CompletedTask;
        }

        private string CreateDeterministicIdFromHash(string payLoad)
        {
            var inputBytes = Encoding.Default.GetBytes(payLoad);
            using (var provider = new MD5CryptoServiceProvider())
            {
                var hashBytes = provider.ComputeHash(inputBytes);
                return new Guid(hashBytes).ToString();
            }
        }

        [FunctionName(nameof(ServiceBusListener))]
        public async Task ServiceBusListener([ServiceBusTrigger("mytopic", "testsub", Connection = "ServiceBusConnectionStringToMyTopic")] string triggerMessage, ILogger log)
        {
            log.LogWarning("Retrieving from the service bus");
            System.Console.WriteLine($"From service Bus Listener: {triggerMessage}");
        }
    }
}