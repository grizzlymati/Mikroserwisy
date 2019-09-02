using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IProductServiceClient _productServiceClient;
        private IOrderServiceClient _orderServiceClient;

        public HomeController(IProductServiceClient productServiceClient, IOrderServiceClient orderServiceClient)
        {
            _productServiceClient = productServiceClient;
            _orderServiceClient = orderServiceClient;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult UserPanel(AuthModel authModel)
        {
            string userData = authModel.GetHashCode().ToString();
            if(authModel.Login == "Admin" && authModel.Password == "Admin")
            {
                return View(authModel);
            }
            else if (authModel.Login == "User1" && authModel.Password == "User1")
            {
                return View(authModel);
            }
            else if(authModel.Login == "User2" && authModel.Password == "User2")
            {
                return View(authModel);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Login), "Home");
            }
        }

        public JsonResult GetAllProducts()
        {
            var products = _productServiceClient.GetAllProducts();
            return new JsonResult(products);
        }

        public IActionResult SendOrder(ProductOrderDetails[] orderData)
        {
            Order newOrder = new Order();
            newOrder.OrdersData = JsonConvert.SerializeObject(orderData);
            newOrder.OrderDate = DateTime.Now;
            newOrder.UserId = 0;
            newOrder.StatusCode = 0;

            HttpStatusCode statusCode = _orderServiceClient.CreateNewOrder(newOrder);
            if (statusCode.ToString() == "OK") return Ok();
            return Conflict();
        }

        public IActionResult DeleteProduct(int id)
        {
            HttpStatusCode statusCode = _productServiceClient.DeleteProduct(id);
            if (statusCode.ToString() == "OK") return Ok();
            return Conflict();
        }

        public IActionResult CreateProduct(Product product)
        {
            HttpStatusCode statusCode = _productServiceClient.CreateProduct(product);
            if (statusCode.ToString() == "OK") return Ok();
            return Conflict();
        }

        public JsonResult GetAllOrders()
        {
            var products = _orderServiceClient.GetAllOrders(-1);
            return new JsonResult(products);
        }

        public IActionResult DeleteOrder(int id)
        {
            HttpStatusCode statusCode = _orderServiceClient.DeleteOrder(id);
            if (statusCode.ToString() == "OK") return Ok();
            return Conflict();
        }
    }
}
