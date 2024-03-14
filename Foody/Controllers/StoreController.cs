using Foody.DTOs;
using Foody.Service.Implementations;
using Foody.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet("Find_Store_By_Name")]
        public async Task<IActionResult> GetStoreByName(string storeName)
        {
            var store = await _storeService.GetStoreByName(storeName);
            if (!store.IsSuccessful) return NotFound(store);
            return Ok(store);
        }

        [HttpGet("Find-Store-By-Id")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var store = await _storeService.GetStoreById(id);
            if (!store.IsSuccessful) return NotFound(store);
            return Ok(store);
        }
        [HttpPost("GetAllStores")]
        public async Task<IActionResult> GetAllStores(SearchParameter searchQuery)
        {
            if (searchQuery.PageSize <= 0) searchQuery.PageSize = 10;
            if (searchQuery.PageNumber <= 0) searchQuery.PageNumber = 1;
            var stores = await _storeService.GetAllStores(searchQuery);
            if (!stores.IsSuccessful) return NotFound(stores);
            return Ok(stores);
        }

        [HttpPost("Add-Store")]
        public async Task<IActionResult> AddStore(AddStoreDto storeDto)
        {
            var result = await _storeService.AddStore(storeDto);
            if (!result.IsSuccessful) return BadRequest(result);
            return Ok(result);
        }


        [HttpDelete("delete-store/{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var result = await _storeService.DeleteStore(id);
            if (!result.IsSuccessful) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("update-store")]
        public async Task<IActionResult> UpdateStore(UpdateStoreDto updateStoreDto)
        {
            var result = await _storeService.UpdateStore(updateStoreDto);
            if (!result.IsSuccessful) return BadRequest(result);
            return Ok(result);
        }
    }
}
