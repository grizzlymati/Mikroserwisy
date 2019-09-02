using System.Collections.Generic;
using OrderService.Models;
using OrderService.DBContext;
using System.Linq;
using OrderService.Enums;
using OrderService.Events.Interfaces;
using OrderService.Events;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderContext _dbContext;
        private ICommandEventConverter _converter;
        private IEventEmitter _eventEmitter;

        public OrderRepository(ICommandEventConverter converter, IEventEmitter eventEmitter, IOrderContext orderContext)
        {
            _dbContext = orderContext;
            _converter = converter;
            _eventEmitter = eventEmitter;
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
                ReleasedProductsDataEvent releasedProductsDataEvent = new ReleasedProductsDataEvent() { ProductsDetails = _converter.CommandToEvent(order.OrdersData) };
                _eventEmitter.EmitReleasedProductsDataEvent(releasedProductsDataEvent);

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
            TakenProductsDataEvent takenProductsDataEvent = new TakenProductsDataEvent() { ProductsDetails = _converter.CommandToEvent(order.OrdersData) };
            _eventEmitter.EmitTakenProductsDataEvent(takenProductsDataEvent);

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
