using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        //public static implicit operator CategoryResponseDto(Response<CategoryResponseDto> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
