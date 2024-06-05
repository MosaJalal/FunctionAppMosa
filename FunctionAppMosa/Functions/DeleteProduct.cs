using FunctionAppMosa.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FunctionAppMosa.Functions
{
    
    
        public class DeleteProduct
        {
            private readonly ILogger<DeleteProduct> _logger;
            private readonly AppDbContext _context;

            public DeleteProduct(ILogger<DeleteProduct> logger, AppDbContext context)
            {
                _logger = logger;
                _context = context;
            }

            [Function("DeleteProduct")]
            public async Task<IActionResult> RemoveProduct([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "pet/{id}")] HttpRequest req, int id)
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                string functionKey = req.Headers["x-functions-key"];
                string expectedKey = Environment.GetEnvironmentVariable("Labb3SecretKey");

                if (functionKey != expectedKey)
                {
                    return new UnauthorizedResult();
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null) return new NotFoundResult();

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
}
