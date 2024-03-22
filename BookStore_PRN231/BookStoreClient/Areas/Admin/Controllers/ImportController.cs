using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
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
        public static List<ImportDetailCreateDto> ImportDetailList { get; set; }

        public ImportController(IHttpContextAccessor httpContextAccessor)
        {
            importApiService = new ImportApiService();
            productApiService = new ProductApiService();
            _httpContextAccessor = httpContextAccessor;
            ImportDetailList = new List<ImportDetailCreateDto>();
        }
        public async Task<IActionResult> Index()
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
            
            ViewBag.UserName = username;

            List<ImportDto> list = (await importApiService.GetImports()) != null? (await importApiService.GetImports()) : new List<ImportDto>();
            return View(list);
        }

        [HttpGet("addimport")]
        public async Task<IActionResult> AddImportForm()
        {
            return View(ImportDetailList);
        }

        [HttpGet("addimportdetailform")]
        public async Task<IActionResult> AddImportDetailForm()
        {
            List<BookDto> books = await productApiService.GetBooks();
            ViewBag.Books = books;
            return View();
        }

        [HttpPost("addimportdetailform")]
        public IActionResult AddImportDetail(ImportDetailCreateDto model)
        {
            if (ModelState.IsValid)
            {
                ImportDetailList.Add(model);
            }
            return RedirectToAction("AddImportForm");
        }
    }
}
