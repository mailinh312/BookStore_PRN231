using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/order")]
    public class OrderController : Controller
    {
        private readonly OrderApiService orderApiService;
        public OrderController()
        {
            orderApiService = new OrderApiService();
        }
        public async Task<IActionResult> Index(string? search)
        {
            List<StatusDto> status = await orderApiService.GetAllStatus();
            List<OrderDto> orders = await orderApiService.GetOrders();
            if (search != null)
            {
                try
                {
                    int orderId = Convert.ToInt32(search);
                    orders = orders.Where(x => x.OrderId == orderId).ToList();

                }
                catch (Exception ex)
                {
                    orders = new List<OrderDto>();
                }
                ViewBag.searchString = search;
            }

            ViewBag.Status = status;
            return View(orders);
        }

        [HttpGet("detailorder")]
        public async Task<IActionResult> DetailOrder(int orderId)
        {
            List<OrderDetailDto> orderDetailDtos = await orderApiService.GetOrderDetailByOrderId(orderId);
            return View(orderDetailDtos);
        }

        [HttpPost("updateorderstatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, int sid)
        {
            try
            {

                HttpResponseMessage response = await orderApiService.UpdateOrderStatus(orderId, sid);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công.";

                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật. Vui lòng thử lại.";

                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}
