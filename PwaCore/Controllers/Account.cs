using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PwaCore.Models;
using PwaCore.Services.Interface;

namespace PwaCore.Controllers
{
    public class Account : Controller
    {
        private IProductService _service;

        public Account(IProductService service)
        {
            _service = service;
        }
        
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AccountViewModel userInput,string foo)//login
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/");
            }
            ViewBag.login = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ورود ریدایرکت کنیم

            #region check phone and Email validation
            if (string.IsNullOrEmpty(userInput.Email))
            {

                return View(userInput);
            }
            const string emailRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            bool isValidEmail = Regex.IsMatch(userInput.Email, emailRegex);
            
            const string usaPhoneNumbersRegex = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
            bool isValidPhone = Regex.IsMatch(userInput.Email, usaPhoneNumbersRegex);

            if (!isValidPhone && !isValidEmail)
            {
                ModelState.AddModelError("Email", "فرمت ایمیل یا شماره تلفن اشتباه است");
                return View(userInput);
            }
            #endregion

            try
            {
                HttpClient httpClient = new HttpClient();

                var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LdZ_oseAAAAALqjwGBp92kFvb0JNVmIwRwkmw1E&response= {foo}").Result;

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError("Email", "درخواست شما با خظا مواجه شد لظفا دوباره تلاش کنید");
                    return View(userInput);
                }
                string JSONres = res.Content.ReadAsStringAsync().Result;
                dynamic JSONdata = JObject.Parse(JSONres);

                if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
                {
                    ModelState.AddModelError("Email", "خطا");
                    return View();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
            

            var user = _service.GetUser(userInput.Email);
            if (user == null || user.Password != userInput.Password)
            {
                ModelState.AddModelError("Email", "کاربر با مشخصات وارد شده یافت نشد ");
                return View();
            }

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.MobilePhone,user.PhoneNumber)
                    //new Claim("IsAdmin", user.IsAdmin.ToString())
                };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = userInput.RememberMe
            };


            await HttpContext.SignInAsync(principal, properties);


            return Redirect("/");
            
        }

        [HttpPost]
        public IActionResult Register(AccountViewModel user)//attr name=asp-for...
        {
            //return View("Welcome");
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.register = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ثبت نام ریدایرکت کنیم
                return View("Views/Account/Index.cshtml", user);
            }

            if (_service.IsExistEmail(user.Email))
            {
                ModelState.AddModelError("Email","ایمیل وارد  شده تکراری است");
                return View("Views/Account/Index.cshtml", user);
            }

            #region CheckFor valid Email And Phone Num
            const string emailRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            bool isValidEmail = Regex.IsMatch(user.Email, emailRegex);
            if (!isValidEmail)
            {
                ViewBag.register = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ثبت نام ریدایرکت کنیم
                ModelState.AddModelError("Email", "فرمت ایمیل اشتباه است");
                return View("Views/Account/Index.cshtml", user);
            }
            //const string usePhoneNumbersRegex = @"/^(\+\d{1,3}[- ]?)?\d{10}$/";
            //bool isValidPhone = Regex.IsMatch(user.Email, usePhoneNumbersRegex);

            //if (!isValidPhone)
            //{
            //    ViewBag.register = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ثبت نام ریدایرکت کنیم
            //    ModelState.AddModelError("PhoneNumber", "فرمت  شماره تلفن اشتباه است");
            //    return View("Views/Account/Index.cshtml", user);
            //}


            #endregion

            _service.AddUser(user);

            return PartialView("Welcome");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
       
    }
}
