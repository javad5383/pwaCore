using PwaCore.Models;

namespace PwaCore.Services.Interface
{
    public interface IProductService
    {
        IEnumerable<Products> GetProducts();

        Products? GetProductById(int productId);
        void AddProduct(Products product,IFormFile mainImg,List<IFormFile> images);
    }
}
