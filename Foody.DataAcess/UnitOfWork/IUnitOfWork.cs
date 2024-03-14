using Foody.DataAcess.CartRepository;
using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.StoreRepository;
using Foody.DataAcess.UserOrderRepository;
using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepo CategoryRepo { get; }
        IProductRepo ProductRepo { get; }
        ICartRepo CartRepo { get; }
        IUserOrderRepo IUserOrderRepo { get; }
        IStoreRepo StoreRepo { get; }



        void Save();
    }
}
