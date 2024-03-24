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
    [Route("admin/import")]
    public class ImportController : Controller
    {
        private readonly ImportApiService importApiService;
        private readonly ProductApiService productApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserApiService userApiService;
        public static List<ImportDetailCreateDto> ImportDetailList = new List<ImportDetailCreateDto>();

        public ImportController(IHttpContextAccessor httpContextAccessor)
        {
            importApiService = new ImportApiService();
            productApiService = new ProductApiService();
            _httpContextAccessor = httpContextAccessor;
            userApiService = new UserApiService();
        }
        public async Task<IActionResult> Index()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			ViewBag.UserName = getUserNameByToken();

            List<ImportDto> list = (await importApiService.GetImports()) != null? (await importApiService.GetImports()) : new List<ImportDto>();
            return View(list);
        }

        [HttpGet("viewdetail")]
        public async Task<IActionResult> ViewDetailImport(int importId)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			List<ImportDetailDto> importDetailDtos = await importApiService.GetImportDetailByImportId(importId);

            ViewBag.UserName = getUserNameByToken();
            return View(importDetailDtos);
        }

        [HttpGet("addimport")]
        public async Task<IActionResult> AddImportForm()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			return View(ImportDetailList);
        }

        [HttpPost("addimport")]
        public async Task<IActionResult> AddImport()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			string username = getUserNameByToken();
            UserDto user = (await userApiService.GetUserByName(username)).FirstOrDefault();
            ImportCreateDto import = new ImportCreateDto();
            decimal? totalPrice = 0;
            if(user != null)
            {
                import.UserId = user.Id;
            }

            foreach(var detail in ImportDetailList)
            {
                totalPrice += detail.Quantity * detail.InputPrice;
            }
            import.TotalPrice = totalPrice;
            int importId = await importApiService.AddImport(import);


                foreach (var detail in ImportDetailList)
                {
                   await importApiService.AddImportDetail(importId, detail);
                }

            ImportDetailList.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("addimportdetailform")]
        public async Task<IActionResult> AddImportDetailForm()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			List<BookDto> books = await productApiService.GetBooks();
            ViewBag.Books = books;
            ViewBag.UserName = getUserNameByToken();
            return View();
        }

        [HttpPost("addimportdetailform")]
        public IActionResult AddImportDetail(ImportDetailCreateDto model)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			if (ModelState.IsValid)
            {
                ImportDetailList.Add(model);
            }
            return RedirectToAction("AddImportForm");
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
			if (roles.Contains("Stock Manager") || roles.Contains("Administrator"))
			{
				return true;
			}
			return false;
		}
	}
}
