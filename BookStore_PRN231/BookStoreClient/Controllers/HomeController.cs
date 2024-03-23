using Azure;
using BookStoreClient.Models;
using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace BookStoreClient.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private UserApiService userApiService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		ProductApiService productApiService;

		public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			userApiService = new UserApiService();
			productApiService = new ProductApiService();
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IActionResult> Index()
		{
			List<BookDto> books = new List<BookDto>();
			books = (await productApiService.GetBooks()).Where(x => x.Active == true).ToList();
			ViewBag.UserName = getUserNameByToken();
			return View(books);
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

		public async Task<IActionResult> AddTocart(int productId)
		{
			try
			{
				BookDto book = (await productApiService.GetBooks()).FirstOrDefault(x => x.BookId == productId);

				CartItem item = new CartItem
				{
					BookId = productId,
					BookTitle = book.Title,
					BookPrice = book.Price,
					ImageUrl = book.ImageUrl,
					StockQuantity = book.StockQuantity,
					Quantity = 1
				};

				var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();

				var existingItem = cart.Items.FirstOrDefault(x => x.BookId == item.BookId);

				if (existingItem != null)
				{
					existingItem.Quantity += item.Quantity;
				}
				else
				{

					cart.Items.Add(new CartItem
					{
						BookId = item.BookId,
						Quantity = item.Quantity,
						BookPrice = item.BookPrice,
						BookTitle = item.BookTitle,
						ImageUrl = item.ImageUrl,
						StockQuantity = item.StockQuantity
					});
				}

				cart.TotalPrice = UpdateTotalPrice(cart.Items);

				_httpContextAccessor.HttpContext.Session.Set("Cart", cart);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}

		public decimal? UpdateTotalPrice(List<CartItem> cartItems)
		{
			decimal? price = 0;
			foreach (var item in cartItems)
			{
				price += item.Quantity * item.BookPrice;
			}
			return price;
		}

		private string getUserNameByToken()
		{
			string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");
			string username = null;
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