using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(AuthModel authModel)
        {
            string userData = authModel.GetHashCode().ToString();
            return RedirectToAction(nameof(ProductsController.Index),"Products", new { id = userData});
        }
    }
}