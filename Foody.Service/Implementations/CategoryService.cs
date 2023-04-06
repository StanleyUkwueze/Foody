using AutoMapper;
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
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        public Response<CategoryResponseDto> AddCategory(AddCategoryDto categoryDto)
        {
            Response<CategoryResponseDto> response = new Response<CategoryResponseDto>();

            if (categoryDto == null) 
                return new Response<CategoryResponseDto> { IsSuccessful = false, Message = "Please, provide the category you want to add" };

            var catToAdd = _mapper.Map<AddCategoryDto, Category>(categoryDto);

            _unitOfWork.CategoryRepo.Add(catToAdd);
            response.Message = "Category added successfully";
            response.IsSuccessful = true;
            response.StatusCode = 201;

            return response;
        }



        public async Task<Response<string>> DeleteCategory(int Id)
        {
            var catToDelete = _unitOfWork.CategoryRepo.GetFirstOrDefauly(x => x.Id == Id);
            if (catToDelete == null) return new Response<string> { Message = " Category does not exist", IsSuccessful = false };
          await  _unitOfWork.CategoryRepo.Remove(catToDelete);
            return new Response<string> { Message = "Category successfully deleted", IsSuccessful = true };
        }

        public PagedResponse<CategoryResponseDto> GetAllCategories(SearchParameter searchQuery)
        {
            var categories = _unitOfWork.CategoryRepo.GetAll().Paginate(searchQuery.PageNumber, searchQuery.PageSize);
            var catsToReturn = _mapper.Map<PagedResponse<CategoryResponseDto>>(categories);

            return catsToReturn;
        }

        public CategoryResponseDto GetCategoryById(int Id)
        {
            var cat = _unitOfWork.CategoryRepo.GetFirstOrDefauly(c => c.Id == Id);
            var catToReturn =  _mapper.Map<Category, CategoryResponseDto>(cat);

            return catToReturn;
        }

        public async Task<CategoryResponseDto> GetCategoryByName(string categoryName)
        {
            var cat = await _unitOfWork.CategoryRepo.GetCategoryByName(categoryName);
        
            var categoryToreturn = _mapper.Map<Category ,CategoryResponseDto>(cat);
            return categoryToreturn;
        }
    }
}
