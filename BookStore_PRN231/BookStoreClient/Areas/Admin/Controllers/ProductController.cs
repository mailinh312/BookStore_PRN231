using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {

        private readonly ProductApiService productApiService;
        private readonly CategoryApiService categoryApiService;
        private readonly AuthorApiService authorApiService;
        private readonly IWebHostEnvironment _environment;
        public int? selectCid;
        public int? selectAid;
        public ProductController(IWebHostEnvironment environment)
        {
            productApiService = new ProductApiService();
            categoryApiService = new CategoryApiService();
            authorApiService = new AuthorApiService();
            _environment = environment;
        }


        public async Task<IActionResult> Index(int? cid, int? aid, string? search)
        {

            List<CategoryDto> categories = await categoryApiService.GetCategories();
            List<AuthorDto> authors = await authorApiService.GetAuthors();
            List<BookDto> books = await productApiService.GetBooks();

            if (search != null)
            {
                books = books.Where(x => x.Title.Trim().ToUpper().Contains(search.Trim().ToUpper())).ToList();
            }

            if (cid.HasValue)
            {
                if (cid != 0)
                {
                    books = books.Where(x => x.CategoryId == cid).ToList();
                    selectCid = cid;
                }
            }

            if (aid.HasValue)
            {
                if (aid != 0)
                {
                    books = books.Where(x => x.AuthorId == aid).ToList();
                    selectAid = aid;
                }
            }


            ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName", selectCid);
            ViewBag.authors = new SelectList(authors, "AuthorId", "AuthorName", selectAid);
            ViewBag.books = books;
            ViewBag.searchString = search ?? null;

            return View();
        }
        [HttpGet("addproduct")]
        public async Task<IActionResult> AddProductForm()
        {
            List<CategoryDto> categories = await categoryApiService.GetCategories();
            List<AuthorDto> authors = await authorApiService.GetAuthors();
            BookCreateDto book = new BookCreateDto();
            ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName", selectCid);
            ViewBag.authors = new SelectList(authors, "AuthorId", "AuthorName", selectAid);
            return View(book);
        }

        [HttpPost("addproduct")]

        public async Task<IActionResult> AddProduct(BookCreateDto model, IFormFile imageFile)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    if (imageFile != null)
                    {

                        var file = Path.Combine(_environment.WebRootPath, @"Images");
                        string filename_new = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                        using (var fileStream = new FileStream(Path.Combine(file, filename_new), FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                        model.ImageUrl = @"\Images\" + filename_new;
                    }



                    HttpResponseMessage response = await productApiService.CreateBook(model);
                    if (response.IsSuccessStatusCode)
                    {
                        // Sản phẩm được tạo thành công
                        TempData["SuccessMessage"] = "Sản phẩm được tạo thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Lỗi khi tạo sản phẩm
                        TempData["ErrorMessage"] = "Lỗi khi tạo sản phẩm. Vui lòng thử lại.";
                        return RedirectToAction("AddProductForm");
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }
            return RedirectToAction("AddProductForm");
        }

        [HttpGet("updateproduct")]
        public async Task<IActionResult> UpdateProductForm(int bookId)
        {
            List<CategoryDto> categories = await categoryApiService.GetCategories();
            List<AuthorDto> authors = await authorApiService.GetAuthors();
            BookDto book = await productApiService.GetBookById(bookId);
            selectCid = book.CategoryId;
            selectAid = book.AuthorId;
            ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName", selectCid);
            ViewBag.authors = new SelectList(authors, "AuthorId", "AuthorName", selectAid);
            return View(book);
        }

        [HttpPost("updateproduct")]

        public async Task<IActionResult> UpdateProduct(BookDto model, IFormFile imageFile)
        {

            {
                try
                {
                    if (imageFile != null)
                    {

                        var file = Path.Combine(_environment.WebRootPath, @"Images");
                        string filename_new = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                        using (var fileStream = new FileStream(Path.Combine(file, filename_new), FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                        model.ImageUrl = @"\Images\" + filename_new;
                    }

                    model.AuthorName = "";
                    model.CategoryName = "";

                    HttpResponseMessage response = await productApiService.UpdateBook(model);
                    if (response.IsSuccessStatusCode)
                    {
                        // Sản phẩm được tạo thành công
                        TempData["SuccessMessage"] = "Sản phẩm được cập nhật thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Lỗi khi tạo sản phẩm
                        TempData["ErrorMessage"] = "Lỗi khi cập nhật sản phẩm. Vui lòng thử lại.";
                        return RedirectToPage("/admin/product/updateproduct", new { bookId = model.BookId });
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }

            return RedirectToPage("/admin/product/updateproduct", new { bookId = model.BookId });
        }
    }
}
