using ProductService.DBContext;
using ProductService.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IProductContext _dbContext;

        public ProductRepository(IProductContext productContext)
        {
                _dbContext = productContext;
        }

        public void DeleteProduct(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            _dbContext.Products.Remove(product);
            SaveChanges();
        }

        public Product GetProductByID(int productId)
        {
            return _dbContext.Products.Find(productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _dbContext.Products.AsEnumerable();
        }

        public void InsertProduct(Product product)
        {
            _dbContext.Products.Add(product);
            SaveChanges();
        }

        public void UpdateProductsAmount(ProductDetails productDetails)
        {
            Product product = GetProductByID(productDetails.ProductId);
            product.Amount += productDetails.ProductAmount;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
