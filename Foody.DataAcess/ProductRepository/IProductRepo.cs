using Foody.DataAcess.Repository;
using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.CategoryRepository
{
    public interface IProductRepo : IGenericRepository<Product> 
    {
        void Update(Product product);
        Task<Product> GetProductByName(string prodName);
    }
}
