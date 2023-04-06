using Foody.DTOs;
using Foody.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Find-Category_By_Name")]
        public async Task<IActionResult> GetCategoryByName(string catName)
        {
            var cat = await _categoryService.GetCategoryByName(catName);
            return Ok(cat);
        }

        [HttpGet("Find-Category_By_Id")]
        public IActionResult GetCategoryById(int id)
        {
            var cat =  _categoryService.GetCategoryById(id);
            return Ok(cat);
        }
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories(SearchParameter searchQuery)
        {
            var cat = _categoryService.GetAllCategories(searchQuery);
            return Ok(cat);
        }
        [HttpPost("Add-Category")]
        public IActionResult AddCategory(AddCategoryDto categoryDto)
        {
            var result = _categoryService.AddCategory(categoryDto);
            return Ok(result);
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return Ok(result);
        }
    }
}
