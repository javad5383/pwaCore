using PwaCore.Context;
using PwaCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
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
                var fileName = Guid.NewGuid() + mainImg.FileName;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using var stream = new FileStream(imagePath, FileMode.Create);

                mainImg.CopyTo(stream);
                product.MainImg = fileName;

            }

            _context.Add(product);
            _context.SaveChanges();


            if (images != null)
            {
                var imgs = new List<ProductImages>();
                foreach (var item in images)
                {
                    var fileName = Guid.NewGuid() + item.FileName;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using var stream = new FileStream(imagePath, FileMode.Create);

                    item.CopyTo(stream);
                    var proImg = new ProductImages()
                    {
                        ImgName = fileName,
                        ProductId = product.Id

                    };
                    imgs.Add(proImg);


                }
                _context.AddRange(imgs);
                _context.SaveChanges();
            }
        }

        public void AddUser(AccountViewModel user)
        {
            Users newUser = new()
            {
                Email = user.Email,
                Password = user.Password,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public Users? GetUser(string userInput)
        {
            if (!string.IsNullOrEmpty(userInput))
            {
                return _context.Users.SingleOrDefault(u => u.Email == userInput || u.PhoneNumber == userInput);
            }

            return null;
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int PageCount()
        {
            var count = _context.Products.Count();
            int pageNumber = count / 3;
            if (count % 3 > 0)
            {
                pageNumber += 1;
            }
            return pageNumber;
        }

        public Cart GetCart(int userId)
        {
            return _context.Cart
                .Include(d => d.CartDetails)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.UserId == userId && !c.IsFinally)!;//!=> for stop warning
        }



        public Products? GetProductById(int productId)
        {
            var pro = _context.Products
                .Include(i => i.ProductImages)
                .SingleOrDefault(p => p.Id == productId);
            return pro;

        }

        public IEnumerable<Products> GetProducts(int pageId)
        {
            var skip = pageId * 3;
            return _context.Products.Skip(skip).Take(3);
        }
        public IEnumerable<Products> GetProducts(string skip)
        {

            return _context.Products.Skip(int.Parse(skip) * 3).Take(3);
        }

        public Cart AddToCart(int userId, int productId, int quantity)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (user != null && product != null && quantity != 0)
            {
                var cart = _context.Cart.FirstOrDefault(c => c.UserId == user.Id && !c.IsFinally);
                if (cart == null)
                {
                    Cart newCart = new()
                    {
                        UserId = userId
                    };
                    _context.Cart.Add(newCart);
                    _context.SaveChanges();
                    var newCartDetail = new CartDetail()
                    {
                        CartId = newCart.Id,
                        ProductId = product.Id,
                        Quantity = quantity,
                        Price = product.Price
                    };
                    newCart.TotalPrice = product.Price;
                    newCart.TotalQuantity = quantity;
                    _context.CartDetails.Add(newCartDetail);
                    _context.SaveChanges();



                }
                else
                {
                    var cartDetail = _context.CartDetails
                        .FirstOrDefault(c => c.CartId == cart.Id && c.ProductId == product.Id);
                    if (cartDetail != null)
                    {
                        cartDetail.Quantity += quantity;
                        cartDetail.Price = product.Price * quantity;
                        _context.SaveChanges();
                        cart.TotalPrice = _context.CartDetails
                            .Where(cd => cd.CartId == cart.Id)
                            .Sum(p => p.Price);

                        cart.TotalQuantity = _context.CartDetails
                            .Where(cd => cd.CartId == cart.Id)
                            .Sum(q => q.Quantity);
                        _context.SaveChanges();
                    }
                    else
                    {
                        CartDetail newCartDetail = new()
                        {
                            CartId = cart.Id,
                            ProductId = productId,
                            Quantity = quantity,
                            Price = product.Price
                        };
                        cart.TotalPrice += newCartDetail.Price;
                        cart.TotalQuantity += newCartDetail.Quantity;
                        _context.CartDetails.Add(newCartDetail);
                        _context.SaveChanges();
                    }


                }


                return cart;

            }


            return null;
        }

        public void FinishPayment(int userId, int cartId)
        {
            _context.Cart.FirstOrDefault(c => c.UserId == userId && c.Id == cartId)!.IsFinally = true;
            _context.SaveChanges();
        }

        public List<Products> GetRandomProducts()
        {
            var countProducts = _context.Products.Count();
            var randomSkip = new Random();
            var skip = randomSkip.Next(0, countProducts - 5);

            return _context.Products.Skip(skip).Take(4).ToList();


        }
    }
}
