
using System;

namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public int StatusCode { get; set; }
        public string OrdersData { get; set; }
    }

}
