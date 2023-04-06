using AutoMapper;
using Foody.DataAcess.UnitOfWork;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        public async Task<Response<ProductResponseDto>> AddProduct(AddProductDto productDto)
        {
            Response<ProductResponseDto> response = new Response<ProductResponseDto>();

            if (productDto == null) return new Response<ProductResponseDto> { Data = null, IsSuccessful = false, Message = "Kindly provide data needed" };
           

            var productId =  _unitOfWork.CategoryRepo.GetFirstOrDefauly(x => x.Name == productDto.CategoryName);

            if (productId != null)
            {
                var productToAdd =  _mapper.Map<AddProductDto, Product>(productDto);
                productToAdd.CategoryId = productId.Id;

               await _unitOfWork.ProductRepo.Add(productToAdd);

                response.IsSuccessful = true;
                response.Message = "Product Added successfully";
                return response;

            }

            response.IsSuccessful = false;
            response.Message = "Category name does not exist";
            return response;

        }

        public async Task<Response<string>> DeleteProduct(int Id)
        {
            var productToDelete = _unitOfWork.ProductRepo.GetFirstOrDefauly(x => x.Id == Id);
            if(productToDelete != null)
            {
               await _unitOfWork.ProductRepo.Remove(productToDelete);
                return new Response<string> { IsSuccessful = true, Message = "Product deleted successfully" };
            }

              return new Response<string> { IsSuccessful = false, Message = "Oops!! Product does not exist" }; 
        }

        public PagedResponse<ProductResponseDto> GetAllProducts(SearchParameter searchQuery)
        {
            var Products = _unitOfWork.ProductRepo.GetAll().Paginate(searchQuery.PageNumber, searchQuery.PageSize);
            var ProductToReturn = _mapper.Map<PagedResponse<ProductResponseDto>>(Products);

            return ProductToReturn;
        }

        public PagedResponse<ProductResponseDto> GetFilterdProducts(SearchParameter query)
        {
            var result = _unitOfWork.ProductRepo.Search(query.Query).Paginate(query.PageNumber, query.PageSize);

            var ProductToReturn = _mapper.Map<PagedResponse<ProductResponseDto>>(result);
            return ProductToReturn;
        }

        public ProductResponseDto GetProductById(int Id)
        {
            var Product = _unitOfWork.ProductRepo.GetFirstOrDefauly(c => c.Id == Id);
            var ProductToReturn =  _mapper.Map<ProductResponseDto>(Product);

            return ProductToReturn;
        }

        public async Task<ProductResponseDto> GetProductByName(string ProductName)
        {
            var Product = await _unitOfWork.ProductRepo.GetProductByName(ProductName);
        
            var ProductToreturn = _mapper.Map<ProductResponseDto>(Product);
            return ProductToreturn;
        }

    }
}
