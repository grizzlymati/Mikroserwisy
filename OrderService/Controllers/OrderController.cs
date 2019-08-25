using Microsoft.AspNetCore.Mvc;
using OrderService.Events;
using OrderService.Models;
using OrderService.Repository;
using System.Transactions;
using OrderService.Events.Interfaces;
using System.Collections.Generic;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderRepository _orderRepository;
        private ICommandEventConverter _converter;
        private IEventEmitter _eventEmitter;

        public OrderController(IOrderRepository orderRepository, ICommandEventConverter converter, IEventEmitter eventEmitter)
        {
            _orderRepository = orderRepository;
            _converter = converter;
            _eventEmitter = eventEmitter;
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int userId)
        {
          //  int statusCode = _orderRepository.DeleteOrder(userId);
           // string ordersData = _orderRepository.GetOrdersDataByID(userId);
            ReleasedProductsDataEvent releasedProductsDataEvent = new ReleasedProductsDataEvent() { ProductsDetails = new List<ProductDetails>() { new ProductDetails() { ProductAmount = 21, ProductId = 37} } };//_converter.CommandToEvent(ordersData) as ReleasedProductsDataEvent;
            _eventEmitter.EmitReleasedProductsDataEvent(releasedProductsDataEvent);

            TakenProductsDataEvent takenProductsDataEventEvent= new TakenProductsDataEvent() { ProductsDetails = new List<ProductDetails>() { new ProductDetails() { ProductAmount = 9, ProductId = 11 } } };//_converter.CommandToEvent(ordersData) as ReleasedProductsDataEvent;
            _eventEmitter.EmitTakenProductsDataEvent(takenProductsDataEventEvent);

            var order = _orderRepository.GetOrdersByUserID(userId); 
            return new OkObjectResult(order);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            using (var scope = new TransactionScope())
            {
                _orderRepository.InsertOrder(order);
                scope.Complete();
                TakenProductsDataEvent takenProductsDataEvent = _converter.CommandToEvent(order.OrdersData) as TakenProductsDataEvent;
                _eventEmitter.EmitTakenProductsDataEvent(takenProductsDataEvent);
                return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] int orderId, int orderStatusCode)
        {
            using (var scope = new TransactionScope())
            {
                _orderRepository.UpdateOrder(orderId, orderStatusCode);
                scope.Complete();
                return new OkResult();
            }
        }

        [HttpDelete("{id}", Name="Delete")]
        public IActionResult Delete(int orderId)
        {
            int statusCode = _orderRepository.DeleteOrder(orderId);
            string ordersData = _orderRepository.GetOrdersDataByID(orderId);
            ReleasedProductsDataEvent releasedProductsDataEvent = _converter.CommandToEvent(ordersData) as ReleasedProductsDataEvent;
            _eventEmitter.EmitReleasedProductsDataEvent(releasedProductsDataEvent);
            return Ok(statusCode);
        }
    }
}
