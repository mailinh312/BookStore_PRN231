using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/product")]
    public class BookController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private readonly ApiService apiService;
        public int? selectedCid = 0;
        public int? selectedAid = 0;


        public BookController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:5000/api";
            apiService = new ApiService();

        }

        [HttpGet]
        public async Task<IActionResult> Index(int? cid, int? aid)
        {

            List<CategoryDto> categories = await apiService.GetCategories();
            List<AuthorDto> authors = await apiService.GetAuthors();
            List<BookDto> books = await apiService.GetBooks();


            if (cid.HasValue)
            {
                if (cid != 0)
                {
                    books = books.Where(x => x.CategoryId == cid).ToList();
                    selectedCid = cid;
                }
            }

            if (aid.HasValue)
            {
                if(aid != 0)
                {
                    books = books.Where(x => x.AuthorId == aid).ToList();
                    selectedAid = aid;
                } 
            }


            ViewBag.categories = categories;
            ViewBag.selectedCategoryId = cid ?? 0;
            ViewBag.authors = authors;
            ViewBag.selectedAuthorId = aid ?? 0;
            ViewBag.books = books;

            return View();
        }


    }
}
