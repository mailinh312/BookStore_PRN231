using BusinessObjects.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IOrderRepository
    {
        List<OrderDto> GetAllOrders();
        OrderDto GetOrderById(int id);
        Order GetDetailOrderById(int id);

        List<OrderDto> GetOrdersByUserId(string userId);

        void UpdateStatusOrder(int orderId, int statusId);

    }
}
