

using Foody.Model.Models;

namespace Foody.DTOs
{
    public class StoreResponseDto
    {
        public StoreResponseDto()
        {
            Products = new List<Product>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Product> Products { get; set; }
    }
}
