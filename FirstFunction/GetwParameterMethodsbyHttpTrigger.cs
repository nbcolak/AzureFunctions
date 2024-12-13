using System;
using System.IO;
using System.Threading.Tasks;
using FirstFunction.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FirstFunction;

public static class GetwParameterMethodsbyHttpTrigger
{
    [FunctionName("RunAsync")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post",  Route = "customRoute")]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        string responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";

        return new OkObjectResult(responseMessage);
        
    }
    
    [FunctionName("RunwIdAsync")]
    public static Task<IActionResult> RunwIdAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post",  Route = "customRoute/{id}")]
        HttpRequest req,
        ILogger log,
        int id)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        

        var responseMessage = string.IsNullOrEmpty(id.ToString())
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {id}. This HTTP triggered function executed successfully.";

        return Task.FromResult<IActionResult>(new OkObjectResult(responseMessage));
        
    }
    [FunctionName("RunwBodyAsync")]
    public static async Task<IActionResult> RunwBodyAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "customRouteBody")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("HTTP trigger function started processing the request.");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(requestBody))
        {
            log.LogWarning("Request body is empty.");
            return new BadRequestObjectResult("Request body is empty or invalid.");
        }

        Product data;
        try
        {
            data = JsonConvert.DeserializeObject<Product>(requestBody);
            if (data == null)
            {
                log.LogWarning("Deserialization resulted in null object.");
                return new BadRequestObjectResult("Invalid JSON format.");
            }
        }
        catch (JsonException ex)
        {
            log.LogError(ex, "Error deserializing the request body.");
            return new BadRequestObjectResult("Invalid JSON format.");
        }

        log.LogInformation("Deserialization successful. Product data received: {@data}", data);

        return new OkObjectResult(data);
    }
}