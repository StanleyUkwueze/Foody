using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.JWT
{
    public interface IJWTService
    {
        Task<string> GenerateJwtToken(Customer customer);
    }
}
