using Foody.DataAcess.Repository;
using Foody.DTOs;
using Foody.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.CategoryRepository
{
    public class ProductRepo: GenericRepository<Product>, IProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Product> Search(string searchTerm)
        {
            var products = _context.Products.Include(x=>x.Category).AsQueryable();
            //if (string.IsNullOrWhiteSpace(searchTerm))
            //    return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            products = products.Where(e => e.Name.ToLower().Contains(lowerCaseTerm) 
                                    || e.Description.ToLower().Contains(lowerCaseTerm) 
                                    || e.Category.Name.ToLower().Contains(lowerCaseTerm)
                                    || e.Category.Description.ToLower().Contains(lowerCaseTerm));
            return products;
        }

            //public IQueryable<Product> GetFilterdProductss(string query )
            //{
            //    var res = _context.Products.Include(b => b.Category).Where(x => 
            //                          EF.Functions.Like(x.Name, $"%{query}%")
            //                       || EF.Functions.Like(x.Description, $"%{query}%")
            //                       || EF.Functions.Like(x.Category.Description, $"%{query}%")
            //                       || EF.Functions.Like(x.Category.Name, $"%{query}%"))
            //                        .AsQueryable();
            //    return res;
            //}
      

        public async Task<Product> GetProductByName(string prodName)
        {
             return await _context.Products.FirstOrDefaultAsync(c => c.Name == prodName);
        }

        public async void Update(Product product)
        {
            var productToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.Id);

            if(productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.ImageUrl = product.ImageUrl;

                _context.SaveChanges();
            }
        }
    }
}
