using PwaCore.Models;

namespace PwaCore.Services.Interface
{
    public interface IProductService
    {
        IEnumerable<Products> GetProducts(int pageId,string gender);
        IEnumerable<Products> GetProducts(string skip);

        Products? GetProductById(int productId);
        void AddProduct(Products product, IFormFile mainImg, List<IFormFile> images,string gender);

        void AddUser(AccountViewModel user);

        Users? GetUser(string userInput);

        bool IsExistEmail(string email);
        int PageCount(string gender);
        Cart GetCart(int userId);

        Cart AddToCart(int userId, int productId,int quantity);
        void FinishPayment(int userId,int cartId);
        List<Products> GetRandomProducts();
        List<Products> GetSpatial();
    }
}
