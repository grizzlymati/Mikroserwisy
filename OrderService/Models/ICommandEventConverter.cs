using OrderService.Events.Interfaces;

namespace OrderService.Models
{
    public interface ICommandEventConverter
    {
        IProductsResourcesData CommandToEvent(string productsDetails);
    }
}
