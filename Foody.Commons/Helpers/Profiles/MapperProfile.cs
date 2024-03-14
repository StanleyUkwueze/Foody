using AutoMapper;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
namespace Foody.Commons.Helpers.Profiles
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, Customer>();
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<Category, Response<CategoryResponseDto>>();
            CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<PagedResponse<Product>, PagedResponse<ProductResponseDto>>();
            CreateMap<PagedResponse<Category>, PagedResponse<CategoryResponseDto>>();
            CreateMap<AddProductDto, Product>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<Customer, AdminUserDTO>();
            CreateMap<PagedResponse<Customer>, PagedResponse<AdminUserDTO>>();
            CreateMap<Customer, AdminUserDTO>();
            CreateMap<Customer, UserDTO>();
            CreateMap<AddStoreDto, Store>();
            CreateMap<Store, StoreResponseDto>().ReverseMap();
            CreateMap<PagedResponse<Store>, PagedResponse<StoreResponseDto>>();
        }
    }
}
