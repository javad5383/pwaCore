using PwaCore.Models;

namespace PwaCore.Services.Interface
{
    public interface IProductService
    {
        IEnumerable<Products> GetProducts();

        Products? GetProductById(int productId);
        void AddProduct(Products product, IFormFile mainImg, List<IFormFile> images);

        void AddUser(AccountViewModel user);

        Users? GetUser(string userInput);

        bool IsExistEmail(string email);

        Cart GetCart(int userId);

        Cart AddToCart(int userId, int productId,int quantity);
        void FinishPayment(int userId,int cartId);
    }
}
