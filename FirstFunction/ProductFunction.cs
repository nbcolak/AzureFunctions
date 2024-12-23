using System;
using System.IO;
using System.Threading.Tasks;
using FirstFunction.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FirstFunction;

public  class ProductFunction
{
    private readonly AppDbContext _appDbContext;
    private const string Route = "Products";

    public ProductFunction(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [FunctionName("GetProducts")]
    public async Task<IActionResult> GetProducts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Tüm  ürünleri getir");

        var products = await _appDbContext.Product.ToListAsync();

        return new OkObjectResult(products);
    }
    
    [FunctionName("SaveProducts")]
    public async Task<IActionResult> SaveProducts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Ürün Kaydet");

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        var newProduct = JsonConvert.DeserializeObject<Product>(body);

        _appDbContext.Product.Add(newProduct);

        await _appDbContext.SaveChangesAsync();

        return new OkObjectResult(newProduct);
    }

    [FunctionName("UpdateProducts")]
    public async Task<IActionResult> UpdateProducts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Ürün Güncelle");

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        var newProduct = JsonConvert.DeserializeObject<Product>(body);

        _appDbContext.Product.Update(newProduct);

        await _appDbContext.SaveChangesAsync();

        return new NoContentResult();
    }
    
    [FunctionName("DeleteProducts")]
    public async Task<IActionResult> DeleteProducts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")] HttpRequest req,
        ILogger log, int id)
    {
        log.LogInformation("Ürün Sil");

        var product = await _appDbContext.Product.FindAsync(id);

        _appDbContext.Product.Remove(product);

        await _appDbContext.SaveChangesAsync();

        return new NoContentResult();
    }
    
    [FunctionName("ProductFunction")]
    public  async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
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
}