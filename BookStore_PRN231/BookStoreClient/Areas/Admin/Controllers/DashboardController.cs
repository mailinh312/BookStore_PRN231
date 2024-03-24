using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreClient.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("admin/dashboard")]
	public class DashboardController : Controller
	{
		private readonly ProductApiService productApiService;
		private readonly CategoryApiService categoryApiService;
		private readonly AuthorApiService authorApiService;
		private readonly OrderApiService orderApiService;
		private readonly UserApiService userApiService;
		private readonly ImportApiService importApiService;
		private readonly IWebHostEnvironment _environment;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DashboardController(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
		{
			productApiService = new ProductApiService();
			categoryApiService = new CategoryApiService();
			authorApiService = new AuthorApiService();
			orderApiService = new OrderApiService();
			userApiService = new UserApiService();
			importApiService = new ImportApiService();
			_environment = environment;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IActionResult> IndexAsync()
		{
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			ViewBag.UserName = getUserNameByToken();

			List<CategoryDto> categories = await categoryApiService.GetCategories();
			List<AuthorDto> authors = await authorApiService.GetAuthors();
			List<BookDto> books = await productApiService.GetBooks();

			List<BestSellerProduct> Top5BestSellers = await productApiService.GetTop5Books();
			List<Top3Category> Top3Categories = await categoryApiService.GetTop3Categories();
			List<BookDto> RunOutProducts = (await productApiService.GetBooks()).Where(o => o.StockQuantity <= 5).ToList();

			int TotalAccount = await userApiService.GetTotalAccount();
			int NumOfSold = Convert.ToInt32((await orderApiService.GetOrderDetails()).Sum(x => x.Quantity));
			decimal TotalInvestment = (decimal)(await importApiService.GetImports()).Sum(x => x.TotalPrice);
			decimal TotalEarning = (decimal)(await orderApiService.GetOrders()).Sum(x => x.TotalPrice) - TotalInvestment;
			ViewBag.TotalAccount = TotalAccount;
			ViewBag.NumOfSold = NumOfSold;
			ViewBag.TotalEarning = TotalEarning;
			ViewBag.TotalInvestment = TotalInvestment;
			ViewBag.Top5BestSeller = Top5BestSellers;
			ViewBag.Top3Category = Top3Categories;
			ViewBag.RunOutProduct = RunOutProducts;
			return View();
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

		private List<string> getRoleByToken()
		{
			string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");
			if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
			{
				Console.WriteLine("Chuỗi token không được để trống hoặc null.");
				token = "";
			}
			else
			{
				var handler = new JwtSecurityTokenHandler();
				var jwtToken = handler.ReadJwtToken(token);

				if (jwtToken != null)
				{
					// Lấy danh sách các Claims từ token
					var claims = jwtToken.Claims;

					// Lọc ra các Claims có loại là "role" và trích xuất giá trị
					var roles = claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

					return roles;
				}
			}
			return new List<string>();
		}

		private bool IsAccess()
		{
			List<string> roles = getRoleByToken();
			if (roles.Contains("Administrator"))
			{
				return true;
			}
			return false;
		}
	}
}
