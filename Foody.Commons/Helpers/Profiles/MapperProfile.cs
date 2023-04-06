using AutoMapper;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Commons.Helpers.Profiles
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, Customer>();
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<Product, ProductResponseDto>();
            CreateMap<PagedResponse<Product>, PagedResponse<ProductResponseDto>>();
            CreateMap<AddProductDto, Product>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<Customer, AdminUserDTO>();
            CreateMap<PagedResponse<Customer>, PagedResponse<AdminUserDTO>>();
            CreateMap<Customer, UserDTO>();
        }
    }
}
