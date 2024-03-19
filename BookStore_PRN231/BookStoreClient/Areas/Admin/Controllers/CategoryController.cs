using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly CategoryApiService categoryApiService;
        public CategoryController()
        {
          categoryApiService = new CategoryApiService();
        }
        public async Task<IActionResult> Index()
        {
            List<CategoryDto> listCate = await categoryApiService.GetCategories();
            return View(listCate);
        }

        [HttpGet("addcategory")]
        public async Task<IActionResult> AddCategoryForm()
        {
            return View();
        }

        [HttpPost("addcategory")]
        public async Task<IActionResult> AddCategory(CategoryCreateDto model)
        {
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
            CategoryDto model = await categoryApiService.GetCategoryById(categoryId);
            return View(model);
        }

        [HttpPost("updatecategory")]
        public async Task<IActionResult> UpdateCategory(CategoryDto model)
        {

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
    }
}
