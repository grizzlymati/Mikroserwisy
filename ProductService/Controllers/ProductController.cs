using ProductService.Models;
using ProductService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("oldest")]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }

        [HttpGet("latest")]
        public IActionResult Get2()
        {
            var products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }


        // GET: api/Product/5
        [HttpGet("/get2/{id}")]
        public IActionResult Get(int id)
        {
            var product = _productRepository.GetProductByID(id);
            return new OkObjectResult(product);
        }

        // POST: api/Product
        [HttpPost("/post1")]
        public IActionResult Post([FromBody] Product product)
        {
            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }

        // PUT: api/Product/5
        [HttpPut("/put1")]
        public IActionResult Put([FromBody] Product product)
        {
            if (product != null)
            {
                using (var scope = new TransactionScope())
                {
                    // _productRepository.UpdateProduct(product);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("/delete1/{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }


        [HttpDelete("/delete2/{idik}")]
        public IActionResult Delete3(int idik)
        {
            _productRepository.DeleteProduct(2);
            return new OkResult();
        }
    }
}
