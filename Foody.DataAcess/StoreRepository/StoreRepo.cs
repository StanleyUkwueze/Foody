using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.Repository;
using Foody.DTOs;
using Foody.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.StoreRepository
{
    public class StoreRepo : GenericRepository<Store>, IStoreRepo
    {

        private readonly AppDbContext _context;
        public StoreRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetStoreProducts(int storeId)
        {
            var response = await _context.Products.Where(x=>x.StoreId == storeId).ToListAsync();

            return response;
        }
        public async Task<Store> GetStoreByName(string storeName)
        {
            return await _context.Stores.FirstOrDefaultAsync(x=> x.Name == storeName);
        }

        public async Task<Store> Update(UpdateStoreDto storeDto)
        {
            var productToUpdate = await _context.Stores.FirstOrDefaultAsync(c => c.Id == storeDto.Id);

            if (productToUpdate != null)
            {
                productToUpdate.Name = storeDto.Name.Length > 0 ? storeDto.Name : productToUpdate.Name;
                productToUpdate.Description = storeDto.Description.Length > 0 ? storeDto.Description :productToUpdate.Description;

                var isSaved = await _context.SaveChangesAsync();
                if (isSaved > 0) return productToUpdate;
              
            }
            return new Store { };
        }
    }
}
