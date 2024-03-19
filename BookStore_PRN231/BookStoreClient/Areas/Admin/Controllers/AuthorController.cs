using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/author")]
    public class AuthorController : Controller
    {

        private readonly AuthorApiService authorApiService;

        public AuthorController()
        {
            authorApiService = new AuthorApiService();
        }
        public async Task<IActionResult> Index(string? search)
        {
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
            return View();
        }

        [HttpPost("addauthor")]
        public async Task<IActionResult> AddAuthor(AuthorCreateDto model)
        {
            if(model.Bio == null)
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
            AuthorDto author = await authorApiService.GetAuthorById(authorId);
            return View(author);
        }

        [HttpPost("updateauthor")]
        public async Task<IActionResult> UpdateAuthor(AuthorDto model)
        {
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
    }
}
