using FunctionAppMosa.Data;
using FunctionAppMosa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppMosa.Functions
{
    public class AddProduct
    {
        private readonly ILogger<AddProduct> _logger;
        private readonly AppDbContext _context;
        public List<Products> mylist = new List<Products>();



        public AddProduct(ILogger<AddProduct> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("AddProduct")]
        public async Task<IActionResult> PostProduct([HttpTrigger(AuthorizationLevel.Function, "post", Route = "pet")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string functionKey = req.Headers["x-functions-key"];
            string expectedKey = Environment.GetEnvironmentVariable("Labb3SecretKey");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Products>(requestBody);
            mylist.Add(product);
            return new CreatedResult("/products", product);
        }
    }
}
