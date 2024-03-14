using AutoMapper;
using Foody.DataAcess;
using Foody.DataAcess.UnitOfWork;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
            _context = context;
        }

        public async Task<Response<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto)
        {
            Response<AddCategoryDto> response = new Response<AddCategoryDto>();

            if (categoryDto == null) 
                return new Response<AddCategoryDto> { IsSuccessful = false, Message = "Please, provide the category you want to add" };

            var cateGoryExist = await GetCategoryByName(categoryDto.Name);
            if (cateGoryExist.IsSuccessful) return new Response<AddCategoryDto> { IsSuccessful = false, Message = "Category name already taken" };
            var catToAdd = _mapper.Map<AddCategoryDto, Category>(categoryDto);
            catToAdd.DateCreated = DateTime.Now;
            catToAdd.DateUpdated = DateTime.Now;
            catToAdd.ImageUrl = "https://imageurl.com";

       
            var isAdded = await _unitOfWork.CategoryRepo.AddAsync(catToAdd);
            if (isAdded)
            {
                response.Message = "Category added successfully";
                response.IsSuccessful = true;
                response.StatusCode = 201;
                return response;
            }

            response.Message = "Category addition failed";
            response.IsSuccessful = false;
            response.StatusCode = 400;
            return response;

        }

        public async Task<Response<string>> DeleteCategory(int Id)
        {
            if (Id <= 0) return new Response<string>
            {
                Message = "Kindly supply a valid category Id",
                IsSuccessful = false
            };

            var catToDelete = _unitOfWork.CategoryRepo.GetFirstOrDefauly(x => x.Id == Id);
            if (catToDelete == null) return new Response<string> { Message = " Category does not exist", IsSuccessful = false };
           var isRemoved = await  _unitOfWork.CategoryRepo.RemoveAsync(catToDelete);
            if (isRemoved) return new Response<string> { Message = "Category successfully deleted", IsSuccessful = true };

            return new Response<string> { Message = "Category deletion failed", IsSuccessful = false };
        }

        public PagedResponse<CategoryResponseDto> GetAllCategories(int PageNumber = 1, int PageSize = 15)
        {
            var categories = _unitOfWork.CategoryRepo.GetAll().Paginate(PageNumber, PageSize);

            if(categories.Result.Count < 1) return new PagedResponse<CategoryResponseDto> { IsSuccessful = false, Message = "No record found" };
            var catsToReturn = _mapper.Map<PagedResponse<CategoryResponseDto>>(categories);

            catsToReturn.Message = "Category records successfully fetched";
            catsToReturn.IsSuccessful = true;

            return catsToReturn;     
        }

        public Response<CategoryResponseDto> GetCategoryById(int Id)
        {
            var response = new Response<CategoryResponseDto>();
            var cat =  _unitOfWork.CategoryRepo.GetFirstOrDefauly(c => c.Id == Id);

            if (cat == null) return new Response<CategoryResponseDto>
            {
                Message = "No Category record found",
                IsSuccessful = false,
                Errors = new string[] { "No record found" }
            };

            var catToReturn =  _mapper.Map<Category,CategoryResponseDto>(cat);

            response.Message = "Successfully fetched category record";
            response.IsSuccessful = true;
            response.Data = catToReturn;

            return response;
        }

        public async Task<Response<CategoryResponseDto>> GetCategoryByName(string categoryName)
        {
            var response = new Response<CategoryResponseDto>();
            if (string.IsNullOrWhiteSpace(categoryName)) return new Response<CategoryResponseDto>
            {
                Message = "Kindly provide a category name",
                IsSuccessful = false,
                Errors = new string[] { "No Category name provided" }
            };

            var cat = await _unitOfWork.CategoryRepo.GetCategoryByName(categoryName);
            if (cat == null) return new Response<CategoryResponseDto>
            {
                Message = "No Category record found",
                IsSuccessful = false,
                Errors = new string[] { "No record found" }
            };

            var categoryToreturn = _mapper.Map<Category ,CategoryResponseDto>(cat);

            response.Message = "Successfully fetched category record";
            response.IsSuccessful = true;
            response.Data = categoryToreturn;

            return  response;   
        }
    }
}
