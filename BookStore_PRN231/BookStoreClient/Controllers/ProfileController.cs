using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreClient.Controllers
{
    public class ProfileController : Controller
    {
        private UserApiService userApiService;
        private OrderApiService orderApiService;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public int? selectCid;
        public int? selectAid;
        public ProfileController(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            userApiService = new UserApiService();
            orderApiService = new OrderApiService();
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> Index()
        {
            string username = getUserNameByToken();
            if (username == null)
            {
                return Redirect("/login");
            }
            UserDto user = (await userApiService.GetUserByName(username)).FirstOrDefault();
            ViewBag.UserName = username;
            return View(user);
        }

        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUser(UserDto model)
        {
            UserDto user = (await userApiService.GetUserByName(model.UserName)).FirstOrDefault();
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.Fullname = model.Fullname;
            HttpResponseMessage response = await userApiService.UpdateUser(user);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet("allorders")]
        public async Task<IActionResult> AllOrders()
        {
            string username = getUserNameByToken();
            if (username == null)
            {
                return Redirect("/login");
            }
            UserDto user = (await userApiService.GetUserByName(username)).FirstOrDefault();
            List<OrderDto> list = await orderApiService.GetOrdersByUserId(user.Id);
            ViewBag.UserName = username;
            return View(list);
        }
        [HttpGet("orderdetail")]
        public async Task<IActionResult> DetailOrder(int orderId)
        {
            string username = getUserNameByToken();
            if (username == null)
            {
                return Redirect("/login");
            }
            ViewBag.UserName = username;
            List<OrderDetailDto> list = await orderApiService.GetOrderDetailByOrderId(orderId);
            return View(list);
        }
        private string getUserNameByToken()
        {
            string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");
            var username = "";
            if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
            {
                username = null;
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
