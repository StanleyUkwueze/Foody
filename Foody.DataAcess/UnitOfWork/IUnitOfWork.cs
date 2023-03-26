using Foody.DataAcess.CategoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepo CategoryRepo { get; }
        IProductRepo ProductRepo { get; }

        void Save();
    }
}
