

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
       
    }
}
