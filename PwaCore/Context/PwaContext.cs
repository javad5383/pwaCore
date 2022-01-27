

using Microsoft.EntityFrameworkCore;

using PwaCore.Models;


namespace PwaCore.Context
{
    public class PwaContext: DbContext
    {

        public PwaContext(DbContextOptions<PwaContext> options) : base(options)
        {

        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }


    }
}
