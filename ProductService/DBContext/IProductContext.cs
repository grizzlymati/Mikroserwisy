using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.DBContext
{
    public interface IProductContext
    {
        DbSet<Product> Products { get; set; }
        int SaveChanges();
    }
}
