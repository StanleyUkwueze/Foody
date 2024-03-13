using Foody.DataAcess.CartRepository;
using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.Repository;
using Foody.DataAcess.UnitOfWork;
using Foody.DataAcess.UserOrderRepository;
using Foody.Service.Implementations;
using Foody.Service.Interfaces;
using Foody.Service.JWT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection Services)
         {
                Services.AddScoped<IJWTService, JWTService>();
                Services.AddScoped<IProductRepo, ProductRepo>();
                Services.AddScoped<IProductService, ProductService>();
                Services.AddScoped<ICategoryRepo, CategoryRepo>();
                Services.AddScoped<ICategoryService, CategoryService>();
                Services.AddScoped<IUnitOfWork, UnitOfWork>();
                Services.AddScoped<IMailService, MailService>();
                Services.AddScoped<IphotoService, PhotoService>();
                Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                Services.AddScoped<IAuthService, AuthService>();
                Services.AddScoped<IUserService, UserService>();
                Services.AddScoped<ICartRepo, CartRepo>();
                Services.AddScoped<IUserOrderRepo, UserOrderRepo>();
                Services.AddScoped<IOrderService, OrderService>();

                Services.AddCors(opt =>
                {
                    opt.AddPolicy("CorsPolicy", policy =>
                    {
                        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
                });
        }
    }
}
