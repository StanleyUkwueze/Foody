﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Model.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Products = new List<Product>();
        }
        public int Id { get; set; }
        public int ItemsCount { get; set; }
        public string  CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
    }
}
