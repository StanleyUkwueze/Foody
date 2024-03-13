using Foody.DataAcess.Repository;
using Foody.DTOs;
using Foody.Model.Models;

namespace Foody.DataAcess.CartRepository
{
    public interface ICartRepo
    {
        Response<int> AddToCart(int productId, int qty);
        Task<decimal> CalculateTotalAmount(int cartId);
        Task<Response<string>> ClearCart(int cartId);
        Task<Response<bool>> DoCheckout();
        Task<Response<ShoppingCart>> GetUserCart();
        Task<Response<int>> RemoveFromCart(int productId);
    }
}