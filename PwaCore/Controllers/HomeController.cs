using Microsoft.AspNetCore.Mvc;
using PwaCore.Models;
using PwaCore.Services.Interface;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using ZarinpalSandbox;

namespace PwaCore.Controllers
{
    public class HomeController : Controller
    {
        string merchant = "424baadf-ea4c-4744-b29e-5eb62a855821";
        string amount = "1100";
        string authority;
        string description = "خرید تستی ";
        string callbackurl = "http://localhost:5008/Home/VerifyPayment";

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
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Payment()
        {
            try
            {
                var payment = new Payment(1000000);

                var res = payment.PaymentRequest("خرید اینترنتب", callbackurl,
                    "javad.mohammadi5383@gmail.com",
                    "09383555383");

                if (res.Result.Status == 100)
                {
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return NotFound();

        }

        public IActionResult VerifyPayment()
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var payment = new Payment(1000000);
                var res = payment.Verification(authority).Result;

                if (res.Status == 100)
                {
                    ViewBag.auth = authority;
                    ViewBag.status = "OK";

                    return View();

                }
            }
            else
            {
                ViewBag.status = "NOK";

                return View();
            }

            return NotFound();
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