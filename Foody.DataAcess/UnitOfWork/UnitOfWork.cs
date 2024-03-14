using Foody.DataAcess.CartRepository;
using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.StoreRepository;
using Foody.DataAcess.UserOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryRepo = new CategoryRepo(context);
            ProductRepo = new ProductRepo(context);
            StoreRepo = new StoreRepo(context);
        }
        public ICategoryRepo CategoryRepo { get; private set; }

        public IProductRepo ProductRepo { get; private set; }
        public ICartRepo CartRepo { get; private set; }
        public IUserOrderRepo IUserOrderRepo { get; private set; }
        public IStoreRepo StoreRepo { get; private set; }

        //public void Dispose()
        //{
        //    _context.Dispose();
        //}

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
