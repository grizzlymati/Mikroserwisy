using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.DBContext
{
    public interface IProductContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        int SaveChanges();
    }
}
