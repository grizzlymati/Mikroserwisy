using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.DBContext
{
    public interface IOrderContext
    {
        DbSet<Order> Orders { get; set; }
        int SaveChanges();
    }
}
