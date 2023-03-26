using Foody.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Find-Product_By_Name")]
        public async Task<IActionResult> GetProductByName(string prodName)
        {
            var product = await _productService.GetProductByName(prodName);
            return Ok(product);
        }

        [HttpGet("Find-Product_By_Id")]
        public IActionResult GetProductById(int id)
        {
            var cat = _productService.GetProductById(id);
            return Ok(cat);
        }
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }
    }
}
