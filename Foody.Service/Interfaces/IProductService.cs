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
    public interface IProductService
    {
        Task<Response<ProductResponseDto>> GetProductByName(string categoryName);
        Response<ProductResponseDto> GetProductById(int Id);
        PagedResponse<ProductResponseDto> GetAllProducts(SearchParameter searchQuery);
        Task<Response<string>> DeleteProduct(int Id);
        Task<Response<ProductResponseDto>> UpdateProduct(UpdateProductDto updateProductDto);
        PagedResponse<ProductResponseDto> GetFilterdProducts(SearchParameter query);
        Task<Response<ProductResponseDto>> AddProduct(AddProductDto productDto);
    }
}
