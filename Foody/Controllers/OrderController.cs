using Foody.DataAcess;
using Foody.DataAcess.UserOrderRepository;
using Foody.DTOs;
using Foody.Model.Models;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public OrderController(IUserOrderRepo userOrderRepo, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userOrderRepo = userOrderRepo;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpPost]
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
    }
}
