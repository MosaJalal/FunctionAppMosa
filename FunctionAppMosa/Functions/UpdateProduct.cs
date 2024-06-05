using FunctionAppMosa.Data;
using FunctionAppMosa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppMosa.Functions
{
    public class UpdateProduct
    {
        private readonly ILogger<UpdateProduct> _logger;
        private readonly AppDbContext _context;

        public UpdateProduct(ILogger<UpdateProduct> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("UpdateProduct")]
        public async Task<IActionResult> PutProduct([HttpTrigger(AuthorizationLevel.Function, "put", Route = "pet/{id}")] HttpRequest req, int id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string functionKey = req.Headers["x-functions-key"];
            string expectedKey = Environment.GetEnvironmentVariable("Labb3SecretKey");

            if (functionKey != expectedKey)
            {
                return new UnauthorizedResult();
            }

            var selectedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (selectedProduct == null) return new NotFoundResult();
            else
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedProduct = JsonConvert.DeserializeObject<Products>(requestBody);
                selectedProduct.Name = updatedProduct.Name;
                selectedProduct.Id = updatedProduct.Id;
                selectedProduct.Price = updatedProduct.Price;
                _context.Products.Update(selectedProduct);
                await _context.SaveChangesAsync();
                return new OkObjectResult(selectedProduct);
            }


        }
    }
}