using Azure;
using BookStoreClient.Models;
using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Diagnostics;
using System.Net;

namespace BookStoreClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserApiService userApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            userApiService = new UserApiService();
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public async Task<IActionResult> LoginForm()
        {

            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            HttpResponseMessage response = await userApiService.Login(model);
            if (response.IsSuccessStatusCode)
            {
                // Đọc dữ liệu trả về từ API
                var token = (await response.Content.ReadAsStringAsync()).Trim('"');
                _httpContextAccessor.HttpContext.Session.Set("token", token);
                return RedirectToAction("Index");

            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Xử lý trường hợp không được phép
                return Unauthorized();
            }
            return RedirectToAction("LoginForm");
        }

        [HttpGet("register")]
        public async Task<IActionResult> RegisterForm()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await userApiService.Register(model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Register");
                }
            }
            return RedirectToAction("Register");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}