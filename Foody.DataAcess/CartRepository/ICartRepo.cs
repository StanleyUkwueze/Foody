using Foody.DTOs;
using Foody.Model.Models;

namespace Foody.DataAcess.CartRepository
{
    public interface ICartRepo
    {
        Response<int> AddToCart(int productId, int qty);
        Task<decimal> CalculateTotalAmount(int cartId);
        Response<string> ClearCart(int cartId);
        Task<bool> DoCheckout();
        Task<ShoppingCart> GetUserCart();
        Task<Response<int>> RemoveFromCart(int productId);
    }
}