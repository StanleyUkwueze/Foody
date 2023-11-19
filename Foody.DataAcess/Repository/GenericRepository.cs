using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task<bool> Add(T entity)
        {
            dbSet.Add(entity);
          return await Save();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = dbSet.AsQueryable();
            return query;
        }

        public T GetFirstOrDefauly(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return  query.FirstOrDefault();
        }

        public async Task<bool> Remove(T entity)
        {
           dbSet.Remove(entity);
           return await Save();
        }

        public async Task<bool> RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange();
           return await Save();
        }

        public async Task<bool> Save()
        {
          return await  _context.SaveChangesAsync() > 0? true: false;
        }

        public async Task<bool> Update(T entity)
        {
            dbSet.Update(entity);
            return await Save();
        }
    }
}
