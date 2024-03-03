using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;

namespace BookStoreAPI.Controllers
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

        [HttpGet("AllOrders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                return Ok(_orderRepository.GetAllOrders());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("Order")]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                return Ok(_orderRepository.GetOrderById(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("Detail")]
        public IActionResult GetDetailOrderById(int id)
        {
            try
            {
                return Ok(_orderRepository.GetDetailOrderById(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("UserId")]
        public IActionResult GetOrderByUserId(string id)
        {
            try
            {
                return Ok(_orderRepository.GetOrdersByUserId(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut("Update")]
        public IActionResult UpdateOrderStatus(int orderId, int statusId)
        {
            try
            {
                _orderRepository.UpdateStatusOrder(orderId, statusId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
