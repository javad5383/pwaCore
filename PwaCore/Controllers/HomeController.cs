using Microsoft.AspNetCore.Mvc;
using PwaCore.Models;
using PwaCore.Services.Interface;
using System.Diagnostics;

namespace PwaCore.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _productService = productService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            
            return View();
        }
        public IActionResult Shop()
        {
            var prod = _productService.GetProducts();
            return View(prod.ToList());
        }
        public IActionResult ShopSingle(int productId)
        {
            var pro=_productService.GetProductById(productId);
            return View(pro);
        }

        public IActionResult Getp()
        {
            return Json(_productService.GetProducts().ToList());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}