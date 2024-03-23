using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int AddNewOrder(OrderCreateDto orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);
            order.OrderDate = DateTime.Now;
            order.StatusId = 1;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId;
        }

        public List<OrderDto> GetAllOrders()
        {
            try
            {
                var orders = _context.Orders.Include(x => x.User).Include(x => x.Status).ToList();
                if (!orders.Any())
                {
                    throw new Exception("List is empty!");
                }

                var orderDtos = _mapper.Map<List<OrderDto>>(orders);
                return orderDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<StatusDto> GetAllStatus()
        {
            var statusList = _context.Status.ToList();
            if (!statusList.Any())
            {
                throw new Exception("List is empty!");
            }
            var statusListDto = _mapper.Map<List<StatusDto>>(statusList);
            return statusListDto;
        }

        public OrderDto GetOrderById(int id)
        {
            try
            {
                var order = _context.Orders.Include(x => x.User).Include(x => x.Status).FirstOrDefault(x => x.OrderId == id);
                if (order == null)
                {
                    throw new Exception("Not found order!");
                }
                var orderDto = _mapper.Map<OrderDto>(order);
                return orderDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }

        public List<OrderDto> GetOrdersByUserId(string userId)
        {
            try
            {
                var orders = _context.Orders.Where(x => x.UserId == userId).Include(x => x.User).Include(x => x.Status).ToList();
                if (!orders.Any())
                {
                    throw new Exception("List is empty!");
                }

                var orderDtos = _mapper.Map<List<OrderDto>>(orders);
                return orderDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateStatusOrder(int orderId, int statusId)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
                if (order == null)
                {
                    throw new Exception("Order does not exist!");
                }

                order.StatusId = statusId;
                order.Status = _context.Status.FirstOrDefault(x => x.StatusId == statusId);
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
