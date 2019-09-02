using Newtonsoft.Json;
using OrderService.Events;
using System;
using System.Collections.Generic;

namespace OrderService.Models
{
    public class CommandEventConverter : ICommandEventConverter
    {
        public IList<ProductDetails> CommandToEvent(string productsDetails)
        {
            try
            {
                var details = JsonConvert.DeserializeObject<IList<ProductDetails>>(productsDetails);
                return details;
            }
            catch (Exception ex)
            {
                return default(IList<ProductDetails>);
            }
        }
    }
}
