using Foody.DataAcess;
using Foody.DataAcess.DataSeedClasses;
using Foody.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.Repository;
using Foody.DataAcess.UnitOfWork;
using Foody.Service.JWT;
using AutoMapper;
using Foody.Commons.Helpers.Profiles;
using Foody.Service.Interfaces;
using Foody.Service.Implementations;
using Foody.Service;


var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
// Add services to the container.
//AppDbContext context = new AppDbContext();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(
            config.GetConnectionString("Default")));
builder.Services.AddIdentity<Customer, IdentityRole>(options => {
    options.SignIn.RequireConfirmedEmail = true;
    //others....
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

ServiceExtensions.AddServices(builder.Services);

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                          new string[] {}
                    }
                });
});

var app = builder.Build();
var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Customer>>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// SeedUser.AddUser(db, userMgr).Wait();

//CategorySeeder.AddCategories(db).Wait();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
