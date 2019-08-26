using ProductService.Models;
using ProductService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System.Collections.Generic;

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

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            IEnumerable<Product> products = _productRepository.GetProducts();
            return new OkObjectResult(products);
        }

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return Ok(product);
            }
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }

    }
}
