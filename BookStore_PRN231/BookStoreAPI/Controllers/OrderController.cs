using BusinessObjects.DTO;
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
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
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

        [HttpGet("UserId")]
        public IActionResult GetOrderByUserId(string id)
        {
            try
            {
                return Ok(_orderRepository.GetOrdersByUserId(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult AddNewOrder(OrderCreateDto order)
        {
            try
            {
                return Ok(_orderRepository.AddNewOrder(order));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
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
                return NotFound(ex.Message);
            }
        }

        [HttpGet("AllStatus")]
        public IActionResult GetAllStatus()
        {
            try
            {
                return Ok(_orderRepository.GetAllStatus());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
