using Microsoft.AspNetCore.Mvc;
using PwaCore.Models;
using PwaCore.Services.Interface;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using ZarinpalSandbox;

namespace PwaCore.Controllers
{
    public class HomeController : Controller
    {

        
       readonly string callbackurl = "http://localhost:5008/Home/VerifyPayment";

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
        [Authorize]
        public IActionResult Cart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = _productService.GetCart(userId);
            if (cart!=null)
            {
                return View(cart);
            }

            return View();

        }

        [Authorize]
        public IActionResult AddToCart(int productId,string quantity)
        {
            if (productId==0||string.IsNullOrEmpty(quantity))
            {
                return NotFound();
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _productService.AddToCart(userId, productId, int.Parse(quantity));
            return RedirectToAction("Cart");
        }
        public IActionResult Payment()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userEmail =User.FindFirstValue(ClaimTypes.Email);
                var userPhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
                var amount = _productService.GetCart(userId).TotalPrice;
                var payment = new Payment((int)amount);

                var res = payment.PaymentRequest("خرید اینترنتی", callbackurl,
                    userEmail,
                    userPhoneNumber);

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
        [Authorize]
        public IActionResult VerifyPayment()
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var cart = _productService.GetCart(userId);
                string authority = HttpContext.Request.Query["Authority"];
                var payment = new Payment((int)cart.TotalPrice);
                var res = payment.Verification(authority).Result;

                if (res.Status == 100)
                {
                    _productService.FinishPayment(userId,cart.Id);
                    ViewBag.auth = authority;
                    ViewBag.status = "OK";

                    return View();

                }
               
            }
            else
            {
                ViewBag.auth = HttpContext.Request.Query["Authority"];
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