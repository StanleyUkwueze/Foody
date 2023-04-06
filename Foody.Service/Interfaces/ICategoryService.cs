using Foody.DataAcess.UnitOfWork;
using Foody.DTOs;
using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> GetCategoryByName(string categoryName);
        CategoryResponseDto GetCategoryById(int Id);
        PagedResponse<CategoryResponseDto> GetAllCategories(SearchParameter searchQuery);
        Task<Response<string>> DeleteCategory(int Id);
        Response<CategoryResponseDto> AddCategory(AddCategoryDto categoryDto);
    }
}
