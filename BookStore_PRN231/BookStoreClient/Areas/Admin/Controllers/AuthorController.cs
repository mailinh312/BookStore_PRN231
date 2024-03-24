using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/author")]
    public class AuthorController : Controller
    {

        private readonly AuthorApiService authorApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorController(IHttpContextAccessor httpContextAccessor)
        {
            authorApiService = new AuthorApiService();
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IActionResult> Index(string? search)
        {
            if (!IsAccess())
            {
                return Redirect("/forbidden");
            }
            ViewBag.UserName = getUserNameByToken();

            List<AuthorDto> authorList = await authorApiService.GetAuthors();
            if (search != null)
            {
                authorList = authorList.Where(x => x.AuthorName.ToUpper().Trim().Contains(search.ToUpper().Trim())).ToList();
                ViewBag.searchString = search;
            }

            return View(authorList);

        }


        [HttpGet("addauthor")]
        public IActionResult AddAuthorForm()
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			return View();
        }

        [HttpPost("addauthor")]
        public async Task<IActionResult> AddAuthor(AuthorCreateDto model)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}

			if (model.Bio == null)
            {
                model.Bio = "";
            }
            try
            {
                HttpResponseMessage response = await authorApiService.CreateAuthor(model);
                if (response.IsSuccessStatusCode)
                {
                    // Tác giả được tạo thành công
                    TempData["SuccessMessage"] = "Tác giả được tạo thành công.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Lỗi khi tạo
                    TempData["ErrorMessage"] = "Lỗi khi tạo tác giả. Vui lòng thử lại.";
                    return RedirectToAction("AddAuthorForm");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }
            return RedirectToAction("AddAuthorForm");
        }

        [HttpGet("updateauthor")]
        public async Task<IActionResult> UpdateAuthorForm(int authorId)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			AuthorDto author = await authorApiService.GetAuthorById(authorId);
            ViewBag.UserName = getUserNameByToken();
            return View(author);
        }

        [HttpPost("updateauthor")]
        public async Task<IActionResult> UpdateAuthor(AuthorDto model)
        {
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}

			if (model.Bio == null)
            {
                model.Bio = "";
            }
            try
            {
                HttpResponseMessage response = await authorApiService.UpdateAuthor(model);
                if (response.IsSuccessStatusCode)
                {
                    // Tác giả được update thành công
                    TempData["SuccessMessage"] = "Tác giả được cập nhật thành công.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Lỗi khi update
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật tác giả. Vui lòng thử lại.";
                    return RedirectToAction("UpdateAuthorForm");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }
            return RedirectToAction("UpdateAuthorForm");
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
