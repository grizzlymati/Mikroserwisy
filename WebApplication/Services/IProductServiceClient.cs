using System.Collections.Generic;
using System.Net;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IProductServiceClient
    {
        IEnumerable<Product> GetAllProducts();

        HttpStatusCode CreateProduct(Product product);

        HttpStatusCode DeleteProduct(int id);

    }
}
