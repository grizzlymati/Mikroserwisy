using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DBContext
{
    public interface IOrderContext
    {
        DbSet<Order> Orders { get; set; }
        int SaveChanges();
    }
}
