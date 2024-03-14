using Foody.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Interfaces
{
    public interface IStoreService
    {
        Task<Response<StoreResponseDto>> GetStoreByName(string categoryName);
        Task<Response<StoreResponseDto>> UpdateStore(UpdateStoreDto updateStoreDto);
        Task<Response<StoreResponseDto>> GetStoreById(int Id);
        Task<PagedResponse<StoreResponseDto>> GetAllStores(SearchParameter searchQuery);
        Task<Response<string>> DeleteStore(int Id);
        Task<Response<StoreResponseDto>> AddStore(AddStoreDto categoryDto);
    }
}
