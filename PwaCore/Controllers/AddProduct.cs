using Microsoft.AspNetCore.Mvc;
using PwaCore.Models;
using PwaCore.Services.Interface;

namespace PwaCore.Controllers
{

    public class AddProduct : Controller
    {
        
        private readonly IProductService _productService;

        public AddProduct(IProductService productService)
        {

            _productService = productService;
        }


        public IActionResult Index()
        {
            return View("/Views/Admin/AddProduct.cshtml");
        }
        [HttpPost]
        public IActionResult Index(Products product,string gender,IFormFile mainImg, List<IFormFile> images)
        {

            if (product == null) { return View("Error"); }
            _productService.AddProduct(product, mainImg, images,gender);

            return Redirect("/home/shop");
        }
    }
}
