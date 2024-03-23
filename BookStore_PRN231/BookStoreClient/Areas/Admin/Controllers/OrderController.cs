using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/order")]
    public class OrderController : Controller
    {
        private readonly OrderApiService orderApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public OrderController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            orderApiService = new OrderApiService();
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.UserName = getUserNameByToken();

            List<StatusDto> status = await orderApiService.GetAllStatus();
            List<OrderDto> orders = await orderApiService.GetOrders();
            if (search != null)
            {
                try
                {
                    int orderId = Convert.ToInt32(search);
                    orders = orders.Where(x => x.OrderId == orderId).ToList();

                }
                catch (Exception ex)
                {
                    orders = new List<OrderDto>();
                }
                ViewBag.searchString = search;
            }

            ViewBag.Status = status;
            return View(orders);
        }

        [HttpGet("detailorder")]
        public async Task<IActionResult> DetailOrder(int orderId)
        {
            List<OrderDetailDto> orderDetailDtos = await orderApiService.GetOrderDetailByOrderId(orderId);
            ViewBag.UserName = getUserNameByToken();
            return View(orderDetailDtos);
        }

        [HttpPost("updateorderstatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, int sid)
        {
            try
            {

                HttpResponseMessage response = await orderApiService.UpdateOrderStatus(orderId, sid);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công.";

                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật. Vui lòng thử lại.";

                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
        private string getUserNameByToken()
        {
            string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");
            var username = "";
            if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
            {
                Console.WriteLine("Chuỗi token không được để trống hoặc null.");
                token = "";
            }
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Trích xuất tên người dùng từ payload
                username = jwtToken.Payload["username"]?.ToString();
            }
            return username;
        }
    }
}
