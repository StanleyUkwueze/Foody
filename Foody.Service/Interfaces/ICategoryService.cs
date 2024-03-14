using Foody.DTOs;
namespace Foody.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<CategoryResponseDto>> GetCategoryByName(string categoryName);
        Response<CategoryResponseDto> GetCategoryById(int Id);
        PagedResponse<CategoryResponseDto> GetAllCategories(int PageNumber, int PageSize);
        Task<Response<string>> DeleteCategory(int Id);
        Task<Response<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto);
    }
}
