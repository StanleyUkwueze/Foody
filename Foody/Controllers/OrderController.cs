using Foody.DataAcess;
using Foody.DataAcess.UserOrderRepository;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUserOrderRepo _userOrderRepo;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public OrderController(IUserOrderRepo userOrderRepo, IOrderService orderService, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userOrderRepo = userOrderRepo;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpPost("place-an-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequestModel PlaceOrderRequestModel)
        {
          
            var result = await _userOrderRepo.PlaceOrderAsync(PlaceOrderRequestModel);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("cancel-order/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await _userOrderRepo.CancelOrder(orderId);
            if(result.IsSuccessful) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-order-as-delivere/{orderId}")]
        public async Task<IActionResult> MarkOrderAsDelivere(int orderId)
        {
            var result = await _userOrderRepo.MarkOrderAsDelivered(orderId);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("reinitiate-order/{orderId}")]
        public async Task<IActionResult> ReinitiateAnOrder(int orderId)
        {
            var result = await _orderService.ReInitiateOrder(orderId);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
