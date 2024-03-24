using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly CategoryApiService categoryApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryController(IHttpContextAccessor httpContextAccessor)
        {
            categoryApiService = new CategoryApiService();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			ViewBag.UserName = getUserNameByToken();

            List<CategoryDto> listCate = await categoryApiService.GetCategories();
            return View(listCate);
        }

        [HttpGet("addcategory")]
        public async Task<IActionResult> AddCategoryForm()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			ViewBag.UserName = getUserNameByToken();
            return View();
        }

        [HttpPost("addcategory")]
        public async Task<IActionResult> AddCategory(CategoryCreateDto model)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await categoryApiService.CreateCategory(model);
                    if (response.IsSuccessStatusCode)
                    {
                        // Thể loại được tạo thành công
                        TempData["SuccessMessage"] = "Thể loại được tạo thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Lỗi khi tạo
                        TempData["ErrorMessage"] = "Lỗi khi tạo thể loại. Vui lòng thử lại.";
                        return RedirectToAction("AddCategoryForm");
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }
            return RedirectToAction("AddCategoryForm");
        }

        [HttpGet("updatecategory")]
        public async Task<IActionResult> UpdateCategoryForm(int categoryId)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			CategoryDto model = await categoryApiService.GetCategoryById(categoryId);
            ViewBag.UserName = getUserNameByToken();
            return View(model);
        }

        [HttpPost("updatecategory")]
        public async Task<IActionResult> UpdateCategory(CategoryDto model)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}

			if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await categoryApiService.UpdateCategory(model);
                    if (response.IsSuccessStatusCode)
                    {
                        // Thể loại được tạo thành công
                        TempData["SuccessMessage"] = "Thể loại được cập nhật thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Lỗi khi tạo
                        TempData["ErrorMessage"] = "Lỗi khi cập nhật thể loại. Vui lòng thử lại.";
                        return RedirectToAction("UpdateCategoryForm");
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }
            return RedirectToAction("UpdateCategoryForm");
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
			if (roles.Contains("Order staff") || roles.Contains("Administrator"))
			{
				return true;
			}
			return false;
		}
	}
}
