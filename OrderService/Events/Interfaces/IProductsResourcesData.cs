using System.Collections.Generic;

namespace OrderService.Events.Interfaces
{
    public interface IProductsResourcesData
    {
        IEnumerable<ProductDetails> ProductsDetails { get; set; }
    }
}
