using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Repository;
using System.Transactions;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderRepository _orderRepository;
        

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        //[Route("api/[controller]/GetAllOrders")]
        public IActionResult GetAllOrders(int userId)
        {
            var orders = _orderRepository.GetOrdersByUserID(userId);

            return new OkObjectResult(orders);
        }

        [HttpPost]
        public IActionResult CreateNewOrder([FromBody] Order order)
        {
            using (var scope = new TransactionScope())
            {
                _orderRepository.InsertOrder(order);
                scope.Complete();
                return Ok();
            }
        }

        [HttpPut]
        public IActionResult UpdateOrdersStatus([FromBody] int orderId, int orderStatusCode)
        {
            using (var scope = new TransactionScope())
            {
                _orderRepository.UpdateOrder(orderId, orderStatusCode);
                scope.Complete();
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult DeleteOrder(int orderId)
        {
            int statusCode = _orderRepository.DeleteOrder(orderId);            

            return Ok(statusCode);
        }
    }
}
