using OrderService.Events.Interfaces;
using System.Collections.Generic;

namespace OrderService.Events
{
    public class TakenProductsDataEvent : IProductsResourcesData
    {
        public IEnumerable<ProductDetails> ProductsDetails { get; set; }
    }
}
