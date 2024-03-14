using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
