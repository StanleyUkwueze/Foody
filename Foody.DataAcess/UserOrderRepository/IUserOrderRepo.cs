using Foody.DataAcess.Repository;
using Foody.DTOs;
using Foody.Model.Models;
using System.Threading.Tasks;

namespace Foody.DataAcess.UserOrderRepository
{
    public interface IUserOrderRepo
    {
        Task<Response<Order>> PlaceOrderAsync(PlaceOrderRequestModel PlaceOrderRequestModel);
        Task<Response<string>> MarkOrderAsDelivered(int orderId);
        Task<Response<string>> CancelOrder(int orderId);
        Task<Order> GetUserOrderAsync(int orderId);
        Task<bool> Remove(Order entity);
        Task<bool> Add(Order entity);
        Task<bool> Update(Order entity);
    }
}