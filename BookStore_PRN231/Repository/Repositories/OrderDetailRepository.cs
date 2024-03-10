using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OrderDetailRepository(BookStoreDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public void AddNewOrderDetail(int orderId, OrderDetailCreateDto orderDetailCreateDto)
        {
            try
            {
                OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailCreateDto);
                orderDetail.OrderId = orderId;
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public Cart AddTocart(CartItem item)
        {
            try
            {
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

                    });
                }

                cart.TotalPrice = UpdateTotalPrice(cart.Items);

                _httpContextAccessor.HttpContext.Session.Set("Cart", cart);
                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Cart DeleteFromCart(int id)
        {
            try
            {
                var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
                var existingItem = cart.Items.FirstOrDefault(x => x.BookId == id);
                if (existingItem != null)
                {
                    cart.Items.Remove(existingItem);
                }

                cart.TotalPrice = UpdateTotalPrice(cart.Items);

                _httpContextAccessor.HttpContext.Session.Set("Cart", cart);
                return cart;
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

        public Cart GetCart()
        {
            try
            {
                // Lấy giỏ hàng từ Session
                var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
                return cart;
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
