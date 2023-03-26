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
        public IActionResult GetAllCategories()
        {
            var cat = _categoryService.GetAllCategories();
            return Ok(cat);
        }
    }
}
