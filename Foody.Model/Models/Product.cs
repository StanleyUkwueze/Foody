﻿

namespace Foody.Model.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public Category Category { get; set; }
    }
}
