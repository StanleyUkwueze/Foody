﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class AddProductDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoredId { get; set; }
        public decimal Price { get; set; }
       // public bool IsStock { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public string  CategoryName { get; set; }
    }
}
