using System.Collections.Generic;
using System.Net;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IOrderServiceClient
    {
        IEnumerable<Order> GetAllOrders(int userId);

        HttpStatusCode CreateNewOrder(Order order);

        HttpStatusCode DeleteOrder(int orderId);
    }
}
