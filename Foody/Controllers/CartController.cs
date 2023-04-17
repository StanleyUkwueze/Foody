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
        public IActionResult RemoveFromCart(int cartId)
        {
          var result = _cartRepo.RemoveFromCart(cartId);
            return Ok(result);
        }

        [HttpGet("get-user-cart")]
        public IActionResult GetUserCart()
        {
            var result = _cartRepo.GetUserCart();
            return Ok(result);
        }

        [HttpPost("CheckOut")]
        public IActionResult CheckOut()
        {
            var result = _cartRepo.DoCheckout();
            return Ok(result);
        }
    }
}
