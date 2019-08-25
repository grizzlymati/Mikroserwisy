using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.DBContext
{
    public interface IProductContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        int SaveChanges();
    }
}
