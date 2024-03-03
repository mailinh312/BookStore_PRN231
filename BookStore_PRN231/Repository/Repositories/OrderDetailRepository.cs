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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public OrderDetailRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddNewOrderDetail(OrderDetailCreateDto orderDetailCreateDto)
        {
            try
            {
                OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailCreateDto);
                _context.Add(orderDetail);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<OrderDetailDto> GetAllOrderDetails()
        {
            try
            {
                var orderDetails = _context.OrderDetails.Include(x => x.Book).ToList();
                if (orderDetails.Count <= 0)
                {
                    throw new Exception("List is empty!");
                }

                var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
                return orderDetailsDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<OrderDetailDto> GetOrderDetailsByOrderId(int id)
        {
            try
            {
                var orderDetails = _context.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Book).ToList();
                if (orderDetails.Count <= 0)
                {
                    throw new Exception("List is empty!");
                }

                var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
                return orderDetailsDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
