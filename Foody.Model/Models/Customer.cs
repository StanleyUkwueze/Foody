﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Foody.Model.Models
{
    public class Customer : IdentityUser
    {
        public Customer()
        {
            Orders = new List<Order>();
            Addresses = new List<Address>();
        }


        [StringLength(maximumLength: 50, ErrorMessage = "The property should not have more than {1} characters")]
        public string? FirstName { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "The property should not have more than {1} characters")]
        public string? LastName { get; set; }
        public string publicId { get; set; }
        public string? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public int? ShoppingCartId { get; set; }
        //[ForeignKey("ShoppingCartId")]
        [JsonIgnore]
        public List<ShoppingCart>? ShoppingCart { get; set; }
        [JsonIgnore]
        public List<Address>? Addresses { get; set; }
        [JsonIgnore]
        public List<Order>? Orders { get; set; }
    }
}
