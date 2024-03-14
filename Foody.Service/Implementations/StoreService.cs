using AutoMapper;
using Foody.DataAcess.UnitOfWork;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class StoreService : IStoreService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<StoreResponseDto>> AddStore(AddStoreDto storeDto)
        {
            Response<StoreResponseDto> response = new Response<StoreResponseDto>();

            if (storeDto == null)
                return new Response<StoreResponseDto> { IsSuccessful = false, Message = "Please, provide the store you want to add" };

            var storeToAdd = _mapper.Map<AddStoreDto, Store>(storeDto);
            storeToAdd.DateCreated = DateTime.Now;

            var storeExist = await _unitOfWork.StoreRepo.GetStoreByName(storeDto.Name);
            if (storeExist != null) return new Response<StoreResponseDto> { IsSuccessful = false, Message = "Store with same name already exist", StatusCode = 400 };

            var isAdded = await _unitOfWork.StoreRepo.AddAsync(storeToAdd);
            if (isAdded)
            {
                var addedStore = _mapper.Map<Store, StoreResponseDto>(storeToAdd);
                response.Message = "Store added successfully";
                response.IsSuccessful = true;
                response.StatusCode = 201;
                response.Data = addedStore;
                return response;
            }

            response.Message = "Store addition failed";
            response.IsSuccessful = false;
            response.StatusCode = 400;
            return response;
        }

        public async Task<Response<string>> DeleteStore(int Id)
        {
            if (Id <= 0) return new Response<string>
            {
                Message = "Kindly supply a valid Store Id",
                IsSuccessful = false
            };

            var storeToDelete = _unitOfWork.StoreRepo.GetFirstOrDefauly(x => x.Id == Id);
            if (storeToDelete == null) return new Response<string> { Message = " Store does not exist", IsSuccessful = false };

            //check if it has products
            var storeToDeleteProducts = await _unitOfWork.StoreRepo.GetStoreProducts(storeToDelete.Id);
            if (storeToDeleteProducts!.Count > 0) return new Response<string> { IsSuccessful = false, Message = "You cannot delete a store with atleast one product" };
            var isRemoved = await _unitOfWork.StoreRepo.RemoveAsync(storeToDelete);
            if (isRemoved) return new Response<string> { Message = "Store successfully deleted", IsSuccessful = true };

            return new Response<string> { Message = "Store deletion failed", IsSuccessful = false };
        }

        public async Task<PagedResponse<StoreResponseDto>> GetAllStores(SearchParameter searchQuery)
        {
            var stores = _unitOfWork.StoreRepo.GetAll().Paginate(searchQuery.PageNumber, searchQuery.PageSize);

            if (stores.Result.Count < 1) return new PagedResponse<StoreResponseDto> { IsSuccessful = false, Message = "No record found" };
            var storeToReturn = _mapper.Map<PagedResponse<StoreResponseDto>>(stores);

            foreach (var store in storeToReturn.Result)
            {
                store.Products = await _unitOfWork.StoreRepo.GetStoreProducts(store.Id);
            }
            storeToReturn.Message = "Store records successfully fetched";
            storeToReturn.IsSuccessful = true;

            return storeToReturn;
        }

        public async Task<Response<StoreResponseDto>> GetStoreById(int Id)
        {
            var response = new Response<StoreResponseDto>();    
            var store = _unitOfWork.StoreRepo.GetFirstOrDefauly(c => c.Id == Id);

            if (store == null) return new Response<StoreResponseDto>
            {
                Message = "No Store record found",
                IsSuccessful = false,
                Errors = new string[] { "No record found" }
            };

            store.Products = await _unitOfWork.StoreRepo.GetStoreProducts(store.Id);
            var storeToReturn = _mapper.Map<Store,StoreResponseDto>(store);

            response.Message = "Successfully fetched store record";
            response.IsSuccessful = true;
            response.Data = storeToReturn;


            return response;
        }

        public async Task<Response<StoreResponseDto>> GetStoreByName(string categoryName)
        {

            if (string.IsNullOrWhiteSpace(categoryName)) return new Response<StoreResponseDto>
            {
                Message = "Kindly provide a store name",
                IsSuccessful = false,
                Errors = new string[] { "No Store name provided" }
            };

            var store = await _unitOfWork.StoreRepo.GetStoreByName(categoryName);
            if (store == null) return new Response<StoreResponseDto>
            {
                Message = "No Store record found",
                IsSuccessful = false,
                Errors = new string[] { "No record found" }
            };

            store.Products = await _unitOfWork.StoreRepo.GetStoreProducts(store.Id);
            var categoryToreturn = _mapper.Map<Store, Response<StoreResponseDto>>(store);

            categoryToreturn.Message = "Successfully fetched store record";
            categoryToreturn.IsSuccessful = true;

            return categoryToreturn;
        }

        public async Task<Response<StoreResponseDto>> UpdateStore(UpdateStoreDto updateStoreDto)
        {
            var isStoreUpdated = await _unitOfWork.StoreRepo.Update(updateStoreDto);

            if (isStoreUpdated.Id > 0)
            {
                var storeToreturn = _mapper.Map<StoreResponseDto>(isStoreUpdated);
                return new Response<StoreResponseDto>
                {
                    Message = "Store successfully updated",
                    IsSuccessful = true,
                    StatusCode = 000,
                    Data = storeToreturn
                };
            }

            return new Response<StoreResponseDto>
            {
                Message = "Store update failed",
                IsSuccessful = false,
                StatusCode = 500,
                Errors = new string[] { "Store Update Failed" }
            };

        }
    }
}
