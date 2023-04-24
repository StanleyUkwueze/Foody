using Foody.DTOs;
using Foody.Model.Models;
using System.Threading.Tasks;

namespace Foody.DataAcess.UserOrderRepository
{
    public interface IUserOrderRepo
    {
        Task<Response<Order>> PlaceOrderAsync(PlaceOrderRequestModel PlaceOrderRequestModel);
        Task<Response<string>> CancelOrder(int orderId);
    }
}