using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
       Task<bool> Save();
       IQueryable<T> GetAll();
       T GetFirstOrDefauly(Expression<Func<T, bool>>? filter = null);
       Task<bool> Remove(T entity);
       Task<bool> RemoveRange(IEnumerable<T> entity);
    }
}
