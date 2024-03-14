using Foody.DTOs;
using Foody.Model.Models;
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
            if (!product.IsSuccessful) return NotFound(product);
            return Ok(product);
        }

        [HttpGet("Find-Product_By_Id")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (!product.IsSuccessful) return NotFound(product);
            return Ok(product);
        }
        [HttpPost("GetAllProducts")]
        public IActionResult GetAllProducts(SearchParameter searchQuery)
        {
            if(searchQuery.PageSize <= 0) searchQuery.PageSize = 10;
            if (searchQuery.PageNumber <= 0) searchQuery.PageNumber = 1;
            var products = _productService.GetAllProducts(searchQuery);
            if(!products.IsSuccessful) return NotFound(products);
            return Ok(products);
        }

        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(AddProductDto productDto)
        {
            var result = await _productService.AddProduct(productDto);
            if (!result.IsSuccessful) return BadRequest(result);
            return Ok(result);
        }


        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result.IsSuccessful) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("search")]
        public IActionResult SearchProducts(SearchParameter query)
        {
            var result = _productService.GetFilterdProducts(query);
            if (!result.IsSuccessful) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var result = await _productService.UpdateProduct(updateProductDto);
            if (!result.IsSuccessful) return BadRequest(result);
            return Ok(result);
        }
    }
}
