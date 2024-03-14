using Foody.DataAcess.Repository;
using Foody.DTOs;
using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.StoreRepository
{
    public interface IStoreRepo : IGenericRepository<Store>
    {
        Task<Store> GetStoreByName(string storeName);
        Task<Store> Update(UpdateStoreDto storeDto);
        Task<List<Product>> GetStoreProducts(int storeId);
    }
}
