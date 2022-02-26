using Microsoft.AspNetCore.Mvc;
using PwaCore.Models;
using PwaCore.Services.Interface;

namespace PwaCore.Controllers
{

    public class Product : Controller
    {
        
        private readonly IProductService _productService;

        public Product(IProductService productService)
        {

            _productService = productService;
        }


        public IActionResult Index()
        {
            return View("/Views/Product/AddProduct.cshtml");
        }
        [HttpPost]
        public IActionResult Index(Products product,string gender,IFormFile mainImg, List<IFormFile> images)
        {

            if (product == null) { return View("Error"); }
            _productService.AddProduct(product, mainImg, images,gender);

            //return Redirect("/home/shop");
            return Redirect("/addProduct");
        }

        public IActionResult EditProduct(int id)
        {
           var prod= _productService.GetProductById(id);
           return View(prod);
        }
        [HttpPost]
        public IActionResult EditProduct(Products product)
        {
            var prod = _productService.GetProductById(1);
            return View(prod);
        }
    }
}
