using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;




namespace BlobToEventHub
{

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [EventHub("test", Connection = "EventHubConnectionAppSetting")] IAsyncCollector<string> outputEvents, ILogger log)


        {


            log.LogInformation("C# HTTP trigger function processed a request.");
            if (outputEvents != null)
            {
                log.LogInformation("this shit is null");
            }
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            await outputEvents.AddAsync(name);  //eventhub
            string responseMessage = string.IsNullOrEmpty(name)
                ? "Not Triggered"
                : $"Hello, {name}. Name sent to Event HUB.";
            log.LogInformation("Following name sent to eventhub " + name);
            log.LogInformation($"C# HTTP trigger function Processed ");
            Console.ReadLine();
            return new OkObjectResult(responseMessage);
        }
    }
}
