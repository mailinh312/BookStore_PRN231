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
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private readonly ApiService apiService;
        private readonly IWebHostEnvironment _environment;
        public int? selectCid;
        public int? selectAid;
        public string img;
        public ProductController(IWebHostEnvironment environment)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:5000/api";
            apiService = new ApiService();
            _environment = environment;
        }


        public async Task<IActionResult> Index(int? cid, int? aid, string? search)
        {

            List<CategoryDto> categories = await apiService.GetCategories();
            List<AuthorDto> authors = await apiService.GetAuthors();
            List<BookDto> books = await apiService.GetBooks();

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
            List<CategoryDto> categories = await apiService.GetCategories();
            List<AuthorDto> authors = await apiService.GetAuthors();
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
                    String imgURL = "";
                    if (imageFile != null)
                    {

                        imgURL = imageFile.FileName;
                        var file = Path.Combine(_environment.WebRootPath, "Images", imageFile.FileName);

                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                    }


                    if (imgURL != null)
                    {
                        model.ImageUrl = imgURL;
                    }

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(ProductApiUrl + "/Book/Create", jsonContent);
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
            List<CategoryDto> categories = await apiService.GetCategories();
            List<AuthorDto> authors = await apiService.GetAuthors();
            BookDto book = await apiService.GetBookById(bookId);
            selectCid = book.CategoryId;
            selectAid = book.AuthorId;
            img = book.ImageUrl;
            ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName", selectCid);
            ViewBag.authors = new SelectList(authors, "AuthorId", "AuthorName", selectAid);
            return View(book);
        }

        [HttpPost("updateproduct")]

        public async Task<IActionResult> UpdateProduct(BookDto model, IFormFile imageFile)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    String imgURL = "";
                    if (imageFile != null)
                    {

                        imgURL = imageFile.FileName;
                        var file = Path.Combine(_environment.WebRootPath, "Images", imageFile.FileName);

                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                    }
                    
                    if (imgURL != null)
                    {
                        model.ImageUrl = imgURL;
                    }
                    else
                    {
                        model.ImageUrl = img;
                    }

                    model.AuthorName = "";
                    model.CategoryName = "";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(ProductApiUrl + "/Book/Update", jsonContent);
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
