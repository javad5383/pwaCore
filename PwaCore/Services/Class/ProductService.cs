using PwaCore.Context;
using PwaCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PwaCore.Services.Interface;

namespace PwaCore.Services.Class
{
    public class ProductService : IProductService
    {
        private PwaContext _context;
        public ProductService(PwaContext context)
        {
            _context = context;
        }

        public void AddProduct(Products product, IFormFile mainImg, List<IFormFile> images)
        {
            if (mainImg != null)
            {
                var fileName = Guid.NewGuid()+mainImg.FileName;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using var stream = new FileStream(imagePath, FileMode.Create);

                mainImg.CopyTo(stream);
                product.MainImg = fileName;
               
            }
            
            _context.Add(product);
             _context.SaveChanges();
        

            if (images != null)
            {
                var imgs=new List<ProductImages>();
                foreach (var item in images)
                {
                    var fileName = Guid.NewGuid()+ item.FileName;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using var stream = new FileStream(imagePath, FileMode.Create);

                    item.CopyTo(stream);
                    var proImg = new ProductImages()
                    {
                        ImgName = fileName ,
                        ProductId = product.Id
                        
                    };
                    imgs.Add(proImg);
                   
                    
                }
                 _context.AddRange(imgs);
                _context.SaveChanges();
            }
        }

        public Products? GetProductById(int productId)
        {
            var pro = _context.Products
                .Include(i => i.ProductImages)
                .SingleOrDefault(p => p.Id == productId);
            return pro;

                
              
        }

        public IEnumerable<Products> GetProducts()
        {
            return _context.Products;
        }

    }
}
