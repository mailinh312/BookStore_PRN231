using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;

namespace BookStoreClient.Controllers
{
    public class ProductDetailController : Controller
    {

        private UserApiService userApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ProductApiService productApiService;

        public ProductDetailController(IHttpContextAccessor httpContextAccessor)
        {
            userApiService = new UserApiService();
            productApiService = new ProductApiService();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int productId)
        {
            BookDto book = await productApiService.GetBookById(productId);
            List<BookDto> BookRelate = (await productApiService.GetBooks()).Where(x => x.CategoryId == book.CategoryId).ToList();
            if (BookRelate.Any())
            {
                Random random = new Random();
                BookRelate = BookRelate.OrderBy(x => random.Next()).Take(3).ToList();
            }
            ViewBag.BookRelate = BookRelate;
            return View(book);
        }

        [HttpPost("addtocart")]
        public async Task<IActionResult> AddTocart(int productId, int quantity)
        {
            try
            {
                BookDto book = await productApiService.GetBookById(productId);

                CartItem item = new CartItem
                {
                    BookId = productId,
                    BookTitle = book.Title,
                    BookPrice = book.Price,
                    ImageUrl = book.ImageUrl,
                    StockQuantity = book.StockQuantity,
                    Quantity = quantity
                };

                var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();

                var existingItem = cart.Items.FirstOrDefault(x => x.BookId == item.BookId);

                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {

                    cart.Items.Add(new CartItem
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        BookPrice = item.BookPrice,
                        BookTitle = item.BookTitle,
                        ImageUrl = item.ImageUrl,
                        StockQuantity = item.StockQuantity
                    });
                }

                cart.TotalPrice = UpdateTotalPrice(cart.Items);

                _httpContextAccessor.HttpContext.Session.Set("Cart", cart);

                return RedirectToAction("Index", new { productId = productId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public decimal? UpdateTotalPrice(List<CartItem> cartItems)
        {
            decimal? price = 0;
            foreach (var item in cartItems)
            {
                price += item.Quantity * item.BookPrice;
            }
            return price;
        }
    }
}
