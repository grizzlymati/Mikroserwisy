using Newtonsoft.Json;
using OrderService.Events.Interfaces;

namespace OrderService.Models
{
    public class CommandEventConverter : ICommandEventConverter
    {
        public IProductsResourcesData CommandToEvent(string productsDetails)
        {
            return JsonConvert.DeserializeObject<IProductsResourcesData>(productsDetails);
        }
    }
}
