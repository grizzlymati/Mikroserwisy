using ProductService.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductService.DBContext
{
    public class ProductContext : DbContext, IProductContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
