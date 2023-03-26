using AutoMapper;
using Foody.DataAcess.UnitOfWork;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public PagedResponse<ProductResponseDto> GetAllProducts()
        {
            var Products = _unitOfWork.ProductRepo.GetAll().Paginate(1,10);
            var ProductToReturn = _mapper.Map<PagedResponse<ProductResponseDto>>(Products);

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
