using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreClient.Controllers
{
    public class CartController : Controller
    {
        private UserApiService userApiService;
        private OrderApiService orderApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ProductApiService productApiService;

        public CartController(IHttpContextAccessor httpContextAccessor)
        {
            userApiService = new UserApiService();
            productApiService = new ProductApiService();
            orderApiService = new OrderApiService();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = getUserNameByToken();
            var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            return View(cart);
        }

        public async Task<IActionResult> DeleteItem(int bookId)
        {
            try
            {
                var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
                var existingItem = cart.Items.FirstOrDefault(x => x.BookId == bookId);
                if (existingItem != null)
                {
                    cart.Items.Remove(existingItem);
                }

                cart.TotalPrice = UpdateTotalPrice(cart.Items);

                _httpContextAccessor.HttpContext.Session.Set("Cart", cart);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantityItem(int bookId, int newQuantity)
        {
            try
            {
                var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
                var existingItem = cart.Items.FirstOrDefault(x => x.BookId == bookId);
                if (existingItem != null)
                {
                    existingItem.Quantity = newQuantity;
                    if (newQuantity < 1)
                    {
                        existingItem.Quantity = 1;
                    }
                }

                cart.TotalPrice = UpdateTotalPrice(cart.Items);

                _httpContextAccessor.HttpContext.Session.Set("Cart", cart);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index");
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

        public async Task<IActionResult> AddTocart(int productId)
        {
            try
            {
                BookDto book = (await productApiService.GetBooks()).FirstOrDefault(x => x.BookId == productId);

                CartItem item = new CartItem
                {
                    BookId = productId,
                    BookTitle = book.Title,
                    BookPrice = book.Price,
                    ImageUrl = book.ImageUrl,
                    StockQuantity = book.StockQuantity,
                    Quantity = 1
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IActionResult> CheckOut(int productId)
        {
            string username = getUserNameByToken();
            if (username == null)
            {
                return Redirect("/login");
            }
            UserDto user = (await userApiService.GetUserByName(username)).FirstOrDefault();

            ViewBag.User = user;
            var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart");
            if (cart.Items.IsNullOrEmpty())
            {
                return RedirectToPage("Cart");
            }

            ViewBag.Cart = cart;
            return View();
        }

        public async Task<IActionResult> CreateOrder(OrderCreateDto model)
        {
            string username = getUserNameByToken();
            if (username == null)
            {
                return Redirect("/login");
            }
            UserDto user = (await userApiService.GetUserByName(username)).FirstOrDefault();
            var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart");
            model.UserId = user.Id;
            model.TotalPrice = cart.TotalPrice;
            int orderId = await orderApiService.CreateOrder(model);

            foreach (var item in cart.Items)
            {
                OrderDetailCreateDto orderDetail = new OrderDetailCreateDto
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.BookPrice * item.Quantity
                };
                orderApiService.CreateOrderDetail(orderId, orderDetail);
            }

            cart = new Cart();
            _httpContextAccessor.HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index");
        }


        private string getUserNameByToken()
        {
            string token = _httpContextAccessor.HttpContext.Session.Get<String>("token") ?? null;
            string username = null;
            if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
            {
                Console.WriteLine("Chuỗi token không được để trống hoặc null.");
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
    }
}
