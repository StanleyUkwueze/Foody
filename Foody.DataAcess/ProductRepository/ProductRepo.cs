using Foody.DataAcess.Repository;
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

        public async Task<Product> GetProductByName(string prodName)
        {
         return await _context.Products.FirstOrDefaultAsync(c => c.Name == prodName);
        }

        public void Update(Product product)
        {
            var productToUpdate = _context.Categories.FirstOrDefault(c => c.Id == product.Id);

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
