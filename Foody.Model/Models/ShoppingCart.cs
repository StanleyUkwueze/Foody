﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        //public int ItemsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
        [JsonIgnore]
        public List<CartDetail> CartDetails { get; set; }
        [JsonIgnore]
        public Customer Customers { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
    }
}
