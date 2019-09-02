using OrderService.Events;
using System.Collections.Generic;

namespace OrderService.Models
{
    public interface ICommandEventConverter
    {
        IList<ProductDetails> CommandToEvent(string productsDetails);
    }
}
