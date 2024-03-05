﻿using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        [HttpGet("AllOrderDetail")]
        public IActionResult GetAllOrderDetails()
        {
            try
            {
                return Ok(_orderDetailRepository.GetAllOrderDetails());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OrderId")]
        public IActionResult GetOrderDetailsByOrderId(int id)
        {
            try
            {
                return Ok(_orderDetailRepository.GetOrderDetailsByOrderId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("Create")]
        public IActionResult AddNewOrderDetail(int orderId, OrderDetailCreateDto orderDetailCreateDto)
        {
            try
            {
                _orderDetailRepository.AddNewOrderDetail(orderId, orderDetailCreateDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Cart")]
        public IActionResult ViewCart()
        {
            try
            {

                return Ok(_orderDetailRepository.GetCart());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(OrderDetailCreateDto item)
        {
            try
            {

                return Ok(_orderDetailRepository.AddTocart(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteFromCart")]
        public IActionResult DeleteFromCart(int id)
        {
            try
            {

                return Ok(_orderDetailRepository.DeleteFromCart(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
