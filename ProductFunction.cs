using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductWebAPI.Services;
using System.IO;
using System.Text.Json;
using AzureFunctionDemo;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProductWebAPI.Functions
{
    public class ProductFunctions
    {
        private readonly ILogger _logger;

        public ProductFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProductFunctions>();
        }

        // GET all products
        [FunctionName("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequestMessage req)
        {
            var products = InMemoryProductService.GetAll();
            return new OkObjectResult(products);
        }

        // GET a product by ID
        [FunctionName("GetProductById")]
        public async Task<IActionResult> GetProductById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id:int}")] HttpRequestMessage req,
            int id)
        {
            var product = InMemoryProductService.GetById(id);
            return product != null ? new OkObjectResult(product) : new NotFoundResult();
        }

        // POST a new product
        [FunctionName("AddProduct")]
        public async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequestMessage req)
        {
            var content = await req.Content.ReadAsStringAsync();
            var newProduct = JsonConvert.DeserializeObject<Product>(content);

            if (newProduct == null)
            {
                return new BadRequestResult();
            }

            InMemoryProductService.Add(newProduct);
            return new CreatedAtRouteResult("GetProductById", new { id = newProduct.ProductID }, newProduct);
        }

        // PUT (update) a product by ID
        [FunctionName("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{id:int}")] HttpRequestMessage req,
            int id)
        {
            var content = await req.Content.ReadAsStringAsync();
            var updatedProduct = JsonConvert.DeserializeObject<Product>(content);

            if (updatedProduct == null || updatedProduct.ProductID != id)
            {
                return new BadRequestResult();
            }

            var success = InMemoryProductService.Update(updatedProduct);
            return success ? new OkObjectResult(updatedProduct) : new NotFoundResult();
        }

        // DELETE a product by ID
        [FunctionName("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{id:int}")] HttpRequestMessage req,
            int id)
        {
            var success = InMemoryProductService.Delete(id);
            return success ? new NoContentResult() : new NotFoundResult();
        }
    }
}
