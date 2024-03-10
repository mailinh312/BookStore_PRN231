using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BookStoreClient.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient client = null;
        private string CategoryApiUrl = "";
        public CategoryController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CategoryApiUrl = "https://localhost:5000/api";
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
