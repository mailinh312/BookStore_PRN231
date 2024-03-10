using BusinessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly IOrderDetailRepository _orderDetailRepository;
        public CartController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        [HttpGet("Cart")]
        public IActionResult ViewCart()
        {
            try
            {
                return Ok(_orderDetailRepository.GetCart());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(CartItem item)
        {
            try
            {

                return Ok(_orderDetailRepository.AddTocart(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteFromCart/Book")]
        public IActionResult DeleteFromCart(int id)
        {
            try
            {
                return Ok(_orderDetailRepository.DeleteFromCart(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
