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
         if(!products.Any())  return products;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            products = products.Where(e => e.Name.ToLower().Contains(lowerCaseTerm) 
                                    || e.Description.ToLower().Contains(lowerCaseTerm) 
                                    || e.Category.Name.ToLower().Contains(lowerCaseTerm)
                                    || e.Category.Description.ToLower().Contains(lowerCaseTerm));
            return products;
        }
      

        public async Task<Product> GetProductByName(string prodName)
        {
            if (string.IsNullOrEmpty(prodName)) return new Product { };

             var result = await _context.Products.FirstOrDefaultAsync(c => c.Name == prodName);
            if (result != null) return result;

               return new Product { };
        }

        public async Task<bool> Update(UpdateProductDto product)
        {
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(c => c.Id == product.Id);

            if(productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.Price = product.Price;

              return await _context.SaveChangesAsync() > 0? true : false;
            }
            return false;
        }
    }
}
