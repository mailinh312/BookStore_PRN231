using BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IOrderDetailRepository
    {
        List<OrderDetailDto> GetAllOrderDetails();

        List<OrderDetailDto> GetOrderDetailsByOrderId(int id);
        void AddNewOrderDetail(int orderId, OrderDetailCreateDto orderDetailCreateDto);

        Cart GetCart();

        Cart AddTocart(OrderDetailCreateDto item);

        Cart DeleteFromCart(int id);
    }
}
