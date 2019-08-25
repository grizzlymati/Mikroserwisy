using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DBContext
{
    public class OrderContext : DbContext, IOrderContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }

    }
}
