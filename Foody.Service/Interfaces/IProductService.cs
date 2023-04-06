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
        Task<ProductResponseDto> GetProductByName(string categoryName);
        ProductResponseDto GetProductById(int Id);
        PagedResponse<ProductResponseDto> GetAllProducts(SearchParameter searchQuery);
        Task<Response<string>> DeleteProduct(int Id);
        PagedResponse<ProductResponseDto> GetFilterdProducts(SearchParameter query);
        Task<Response<ProductResponseDto>> AddProduct(AddProductDto productDto);
    }
}
