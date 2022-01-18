using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
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
           
            return View();
        }
        [HttpPost]
        public IActionResult Index(AccountViewModel user)
        {
            ViewBag.login = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ورود ریدایرکت کنیم

            #region check phone and Email validation
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {

                return View(user);
            }
            const string emailRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            bool isValidEmail = Regex.IsMatch(user.Email, emailRegex);

            const string usaPhoneNumbersRegex = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
            bool isValidPhone = Regex.IsMatch(user.Email, usaPhoneNumbersRegex);

            if (!isValidPhone && !isValidEmail)
            {
                ModelState.AddModelError("Email", "فرمت ایمیل یا شماره تلفن اشتباه است");
                return View(user);
            }


            #endregion

            return BadRequest();
        }

        [HttpPost]
        public IActionResult Register(AccountViewModel user)//attr name=asp-for...
        {
             
            if (!ModelState.IsValid)
            {
                ViewBag.register = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ثبت نام ریدایرکت کنیم
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
            const string usaPhoneNumbersRegex = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}/";
            bool isValidPhone = Regex.IsMatch(user.Email, usaPhoneNumbersRegex);

            if (!isValidPhone)
            {
                ViewBag.register = true; //برای اینکه در ویو متوجه شویم که باید به قسمت ثبت نام ریدایرکت کنیم
                ModelState.AddModelError("PhoneNumber", "فرمت  شماره تلفن اشتباه است");
                return View("Views/Account/Index.cshtml", user);
            }


            #endregion

            _service.AddUser(user);
           
            return Redirect("/Account");
        }
    }
}
