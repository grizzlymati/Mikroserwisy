using ProductService.Models;
using System.Collections.Generic;


namespace ProductService.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductByID(int product);
        void InsertProduct(Product product);
        void DeleteProduct(int productId);
        void UpdateProductsAmount(ProductDetails productDetails);
        void SaveChanges();
    }
}
