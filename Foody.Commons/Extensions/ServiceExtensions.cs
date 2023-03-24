using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.Repository;
using Foody.DataAcess.UnitOfWork;
using Foody.Service;
using Foody.Service.JWT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Commons.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection service)
        {
            service.AddScoped<IJWTService, JWTService>();
            service.AddScoped<IProductRepo, ProductRepo>();
            service.AddScoped<ICategoryRepo, CategoryRepo>();
            service.AddScoped<UnitOfWork>();
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IAuthService,AuthService>();
        }
    }
}
