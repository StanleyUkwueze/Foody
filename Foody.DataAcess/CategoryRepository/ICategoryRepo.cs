using Foody.DataAcess.Repository;
using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.CategoryRepository
{
    public interface ICategoryRepo : IGenericRepository<Category> 
    {
        void Update(Category category);
        Task<Category> GetCategoryByName(string cateName);
    }
}
