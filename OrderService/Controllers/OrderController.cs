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

        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders(int userId)
        {
            var orders = _orderRepository.GetOrdersByUserID(userId);

            return new OkObjectResult(orders);
        }

        [HttpPost("CreateNewOrder")]
        public IActionResult CreateNewOrder([FromBody] Order order)
        {
            using (var scope = new TransactionScope())
            {
                _orderRepository.InsertOrder(order);
                scope.Complete();
                return Ok();
            }
        }

        [HttpPut("UpdateOrdersStatus")]
        public IActionResult UpdateOrdersStatus(OrderStatusModel orderStatus)
        {
            using (var scope = new TransactionScope())
            {
                _orderRepository.UpdateOrder(orderStatus.Id, orderStatus.StatusCode);
                scope.Complete();
                return Ok();
            }
        }

        [HttpDelete("DeleteOrder")]
        public IActionResult DeleteOrder(int orderId)
        {
            int statusCode = _orderRepository.DeleteOrder(orderId);            

            return Ok(statusCode);
        }
    }
}
