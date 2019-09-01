using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult UserPanel(AuthModel authModel)
        {
            string userData = authModel.GetHashCode().ToString();
            //return RedirectToAction(nameof(ProductsController.Index), "Products", new { id = userData });
            return View();
        }

        public JsonResult GetAllProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = 14,
                    Name = "Test1",
                    Description = "DescTest1",
                    Price = 2.1M,
                    Amount = 4
                },
                   new Product()
                {
                    Id = 11,
                    Name = "Test12",
                    Description = "DescTest21",
                    Price = 2.11M,
                    Amount = 5
                }
            };
            return new JsonResult(products);
        }

        public IActionResult SendOrder(ProductOrderDetails[] orderData)
        {
            return Ok();
        }

        public IActionResult DeleteProduct(int id)
        {
            return Ok();
        }

        public IActionResult CreateProduct(Product product)
        {
            return Ok();
        }
    }
}
