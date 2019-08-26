using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductService.DBContext;
using System.IO;

namespace OrderService.DBContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ProductContext>();

            var connectionString = configuration.GetConnectionString("ProductDB");

            builder.UseSqlServer(connectionString);

            return new ProductContext(builder.Options);
        }
    }
}
