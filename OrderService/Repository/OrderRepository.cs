using System.Collections.Generic;
using OrderService.Models;
using OrderService.DBContext;
using System.Linq;
using OrderService.Enums;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderContext _dbContext;

        public OrderRepository()
        {
            _dbContext = new OrderContext(new DbContextOptions<OrderContext>());
        }

        public IEnumerable<Order> GetOrdersByUserID(int userId)
        {
            return _dbContext.Orders.Where(x => x.UserId == userId);
        }

        public int DeleteOrder(int orderId)
        {
            Order order = _dbContext.Orders.Find(orderId);
            if (CanDeleteOrder(order.StatusCode))
            {
                _dbContext.Orders.Remove(order);
                SaveChanges();
                return (int)DeleteOrderStatusCode.DeletedSuccessfuly;
            }
            else
            {
                return (int)DeleteOrderStatusCode.WrongOrderStatus;
            }
        }

        public void InsertOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            SaveChanges();
        }

        public void UpdateOrder(int orderId, int ordersStatusCode)
        {
            Order order = _dbContext.Orders.Find(orderId);
            order.StatusCode = ordersStatusCode;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public string GetOrdersDataByID(int orderId)
        {
            return _dbContext.Orders.Find(orderId).OrdersData;
        }

        private bool CanDeleteOrder(int statusCode)
        {
            return statusCode == (int)OrderStatusCode.Ordered || statusCode == (int)OrderStatusCode.Prepared;
        }

    }
}
