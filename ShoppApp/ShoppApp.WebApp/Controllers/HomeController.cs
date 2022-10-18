using Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ShopApp.Entity;
using ShopApp.Business.Abstract;
using ShoppApp.WebApp.Models;

namespace ShoppApp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()
            };

            return View(productViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View("MyView");
        }
    }
}