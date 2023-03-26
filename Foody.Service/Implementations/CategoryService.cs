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

        public PagedResponse<CategoryResponseDto> GetAllCategories()
        {
            var categories = _unitOfWork.CategoryRepo.GetAll().Paginate(1, 3);
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
