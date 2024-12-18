using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;
using FirstFunction.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FirstFunction;

public static class HttpTriggerBindings
{
    [FunctionName("HttpTriggerBindings")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Azure Table Storage işlem başlatıldı.");
        string connectionString = Environment.GetEnvironmentVariable("MyAzureStorage");
        var tableClient = new TableClient(connectionString, "Product");
        var productEntity = new TableEntity("ProductsPartition", Guid.NewGuid().ToString())
        {
            { "Name", "Product123" },
            { "Price", 49.99 }
        };
        await tableClient.AddEntityAsync(productEntity);
        return new OkObjectResult("Product successfully added to Table Storage.");
        
    }
    
    [FunctionName("HttpTriggerBindingsQueue")]
    [return: Queue("queueproduct", Connection = "MyAzureStorageQueue")]
    public static async Task<Product> RunQueueAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Azure Queue işlem başlatıldı.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        Product newProduct = JsonConvert.DeserializeObject<Product>(requestBody);

        return newProduct;
        
    }
}