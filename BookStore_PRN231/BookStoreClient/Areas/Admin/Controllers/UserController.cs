using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/user")]
    public class UserController : Controller
    {
        private readonly UserApiService userApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            userApiService = new UserApiService();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");

            if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
            {
                Console.WriteLine("Chuỗi token không được để trống hoặc null.");
                return null;
            }
            //// Giải mã token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Trích xuất tên người dùng từ payload
            var username = jwtToken.Payload["username"]?.ToString();
            ViewBag.UserName = username;

            List<UserDto> users = await userApiService.GetUsers();
            return View(users);
        }
        [HttpGet("updateuser")]
        public async Task<IActionResult> UpdateUserForm(string iuserId){
            return View();
        }
    }
}
