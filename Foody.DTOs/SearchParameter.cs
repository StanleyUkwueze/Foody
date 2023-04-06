using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class SearchParameter
    {
        public string Query { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 15;
    }
}
