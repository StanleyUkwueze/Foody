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
    public class CategoryRepo : GenericRepository<Category>, ICategoryRepo
    {
        private readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByName(string cateName)
        {
         return await _context.Categories.FirstOrDefaultAsync(c => c.Name == cateName);
        }

        public void Update(Category category)
        {
            var categoryToUpdate = _context.Categories.FirstOrDefault(c => c.Id == category.Id);

            if(categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Description = category.Description;
                categoryToUpdate.ImageUrl = category.ImageUrl;

                _context.SaveChanges();
            }
        }
    }
}
