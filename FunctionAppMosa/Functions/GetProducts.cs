using FunctionAppMosa.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FunctionAppMosa.Functions
{
    public class GetProducts
    {
        private readonly ILogger<GetProducts> _logger;
        private readonly AppDbContext _context;

        public GetProducts(ILogger<GetProducts> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("GetProducts")]
        public async Task<IActionResult> GetAllProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "pet")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string functionKey = req.Headers["x-functions-key"];
            string expectedKey = Environment.GetEnvironmentVariable("Labb3SecretKey");

            if (functionKey != expectedKey)
            {
                return new UnauthorizedResult();
            }

            var products = await _context.Products.ToListAsync();
            return new OkObjectResult(products);
        }
    }
}
