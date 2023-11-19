using Foody.DataAcess.CartRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpPost("add-to-cart")]
        public IActionResult AddToCart(int productId, int qty)
        {
            var result =  _cartRepo.AddToCart(productId, qty);
            return Ok(result);
        }
        [HttpDelete("remove-item-from-cart")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
          var result = await _cartRepo.RemoveFromCart(productId);
            return Ok(result);
        }

        [HttpGet("get-user-cart")]
        public async Task<IActionResult> GetUserCart()
        {
            var result = await _cartRepo.GetUserCart();
            return Ok(result);
        }

        //[HttpPost("CheckOut")]
        //public async Task<IActionResult>  CheckOut()
        //{
        //    var result = await _cartRepo.DoCheckout();
        //    return Ok(result);
        //}
    }
}
