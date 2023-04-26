using Foody.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Interfaces
{
    public interface IOrderService
    {
        Task<Response<string>> ReInitiateOrder(int orderId);
    }
}
