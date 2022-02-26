using PwaCore.Context;
using PwaCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
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

        public void AddProduct(Products product, IFormFile mainImg, List<IFormFile> images, string gender)
        {
            switch (gender)
            {
                case "Man":
                    {
                        product.ForMan = true;
                        break;
                    }
                case "Woman":
                    {
                        product.ForWoMan = true;
                        break;
                    }
                case "Children":
                    {
                        product.ForChildren = true;
                        break;
                    }
            }

            if (mainImg.Length != 0)
            {
                Random generator = new Random();
                String random = generator.Next(0, 1000000).ToString("D6");
                var fileName = random + mainImg.FileName;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using var stream = new FileStream(imagePath, FileMode.Create);

                mainImg.CopyTo(stream);
                product.MainImg = fileName;

            }

            _context.Add(product);
            _context.SaveChanges();


            if (images.Count != 0)
            {
                Random generator = new Random();
                String random = generator.Next(0, 1000000).ToString("D6");
                var imgs = new List<ProductImages>();
                foreach (var item in images)
                {
                    var fileName = random + item.FileName;

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

        public int PageCount(string gender)
        {
            int count;
            switch (gender)
            {
                case "man":
                    {
                        count = _context.Products.Count(c=>c.ForMan);
                        break;
                    }
                case "woman":
                {
                    count = _context.Products.Count(c => c.ForWoMan);
                    break;
                }
                case "children":
                {
                    count = _context.Products.Count(c => c.ForChildren);
                    break;
                }
                default: count = _context.Products.Count();break;
            }

            //var count = _context.Products.Count();


            int pageNumber = count / 6;
            if (count % 6 > 0)
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

        public IEnumerable<Products> GetProducts(int pageId, string gender)
        {

            var skip = pageId * 6;

            switch (gender)
            {
                case "man": return _context.Products.Where(p => p.ForMan == true).Skip(skip).Take(6); ;

                case "woman": return _context.Products.Where(p => p.ForWoMan == true).Skip(skip).Take(6); ;

                case "children": return _context.Products.Where(p => p.ForChildren == true).Skip(skip).Take(6);

                default: return _context.Products.Skip(skip).Take(6); ;
            }

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
                        Price = (quantity > 1) ? product.Price * quantity : product.Price
                    };
                    newCart.TotalPrice = (quantity > 1) ? product.Price * quantity : product.Price;
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
                        cartDetail.Price += (quantity > 1) ? product.Price * quantity : product.Price;
                        _context.SaveChanges();
                        cart.TotalPrice += (quantity > 1) ? product.Price * quantity : product.Price;

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
                            Price = (quantity > 1) ? product.Price * quantity : product.Price
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

        public List<Products> GetSpatial()
        {
            var countProducts = _context.Products.Count(p=>p.ForWoMan);
            var randomSkip = new Random();
            var skip = randomSkip.Next(0, countProducts - 5);

            return _context.Products.Where(p=>p.ForWoMan).Skip(skip).Take(4).ToList();
        }

        public List<Products> SearchProducts(string input)
        {
            return _context.Products
                .Where(n=>n.Name.Contains(input))
                .ToList()!;
        }
    }
}
