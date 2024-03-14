using AutoMapper;
using CloudinaryDotNet.Actions;
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

            if (productDto == null) return new Response<ProductResponseDto> {IsSuccessful = false, Message = "Kindly provide data needed" };
           

            var productCategory =  _unitOfWork.ProductRepo.GetFirstOrDefauly(x => x.Name.ToLower() == productDto.CategoryName.Trim().ToLower());
            var productStore = _unitOfWork.StoreRepo.GetFirstOrDefauly(x => x.Id == productDto.StoredId);
            if (productCategory == null)
            {
                response.IsSuccessful = false;
                response.Message = "Category name does not exist";
                response.Errors = new string[] { "Category Not Added" };
                return response;
            }
            if (productStore == null) return new Response<ProductResponseDto>
            {
                Message = "Store not found",
                IsSuccessful = false,
            };
                
               var productToAdd =  _mapper.Map<AddProductDto, Product>(productDto);
               productToAdd.CategoryId = productCategory.Id;
               productToAdd.StoreId = productStore.Id;

               var isAdded =  await _unitOfWork.ProductRepo.AddAsync(productToAdd);

              if(isAdded)
                        {
                    var addedProduct = _mapper.Map<Product, ProductResponseDto>(productToAdd);
                    response.IsSuccessful = true;
                    response.Message = "Product Added successfully";
                    response.Data = addedProduct;
                    response.StatusCode = 000;

                   return response;
              }

                response.IsSuccessful = false;
                response.Message = "An error occured while adding the product";
                response.Errors = new string[] { "Product Not Added" };
                return response;
            

          

        }

        public async Task<Response<string>> DeleteProduct(int Id)
        {
            var productToDelete = _unitOfWork.ProductRepo.GetFirstOrDefauly(x => x.Id == Id);
            if(productToDelete != null)
            {
              var isDeleted = await _unitOfWork.ProductRepo.RemoveAsync(productToDelete);
                if(!isDeleted) return new Response<string> { IsSuccessful = true, Message = "Opps! Product could not be deleted", Errors = new string[] { "Product Not Found" } };
                return new Response<string> { IsSuccessful = true, Message = "Product deleted successfully" };
            }

              return new Response<string> { IsSuccessful = false, Message = "Oops!! Product does not exist" }; 
        }

        public PagedResponse<ProductResponseDto> GetAllProducts(SearchParameter searchQuery)
        {
            var Products = _unitOfWork.ProductRepo.GetAll().Paginate(searchQuery.PageNumber, searchQuery.PageSize);
            if (Products.Result.Count < 1) return new PagedResponse<ProductResponseDto> { Message = "No product found", StatusCode = 404 , Errors = new string[] { "Product Not Found" } };

            var ProductToReturn = _mapper.Map<PagedResponse<ProductResponseDto>>(Products);
            ProductToReturn.IsSuccessful = true;
            ProductToReturn.Message = "Successfully fetched all products";
            
            return ProductToReturn;
        }

        public PagedResponse<ProductResponseDto> GetFilterdProducts(SearchParameter query)
        {
            var result = _unitOfWork.ProductRepo.Search(query.Query).Paginate(query.PageNumber, query.PageSize);

            if (result.Result.Count < 1) return new PagedResponse<ProductResponseDto> {Message = "No product found", StatusCode = 404, Errors = new string[] { "Product Not Found" } };

            var ProductToReturn = _mapper.Map<PagedResponse<ProductResponseDto>>(result);
            ProductToReturn.IsSuccessful = true;
            ProductToReturn.Message = "Successfully fetched products";

            return ProductToReturn;
 
        }

        public  Response<ProductResponseDto> GetProductById(int Id)
        {
            var Product = _unitOfWork.ProductRepo.GetFirstOrDefauly(c => c.Id == Id);
            if (Product == null) return new Response<ProductResponseDto> { Message = "No product found", IsSuccessful = false, Errors = new string[] { "Product Not Found" } };

            var ProductToReturn =  _mapper.Map<ProductResponseDto>(Product);

            return new Response<ProductResponseDto>
            {
                Message = "Product Successfully fetched",
                Data = ProductToReturn,
                IsSuccessful = true
            };
        }

        public async Task<Response<ProductResponseDto>> GetProductByName(string ProductName)
        {
            var Product = await _unitOfWork.ProductRepo.GetProductByName(ProductName);
        
            if(Product.Id > 0)
            {
                var ProductToreturn = _mapper.Map<ProductResponseDto>(Product);
                return new Response<ProductResponseDto>
                {
                    Message = "Product successfully fetched",
                    IsSuccessful  = true,
                    Data = ProductToreturn
                };
            }
            return new Response<ProductResponseDto>
            {
                Message = "No available product with the provided product name found",
                IsSuccessful = false,
                StatusCode = 404,
                Errors = new string[] { "Product Not Found"}
            };

        }

        public async Task<Response<ProductResponseDto>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var isProductUpdated = await _unitOfWork.ProductRepo.Update(updateProductDto);

            if (isProductUpdated.Id > 0)
            {
                var ProductToreturn = _mapper.Map<ProductResponseDto>(isProductUpdated);
                return new Response<ProductResponseDto>
                {
                    Message = "Product successfully updated",
                    IsSuccessful = true,
                    StatusCode = 000,
                    Data = ProductToreturn
                };
            }

            return new Response<ProductResponseDto>
            {
                Message = "Product update failed",
                IsSuccessful = false,
                StatusCode = 500,
                Errors = new string[] { "Product Update Failed" }
            };

        }
    }
}
