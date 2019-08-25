using System.Collections.Generic;
using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrdersByUserID(int userId);
        int DeleteOrder(int orderId);
        void SaveChanges();
        void InsertOrder(Order order);
        void UpdateOrder(int orderId, int ordersStatusCode);
        string GetOrdersDataByID(int orderId);
    }
}
