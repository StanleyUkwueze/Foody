using Foody.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class PlaceOrderRequestModel
    {
        public DateTime CheckOutDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }
        //public decimal TotalPrice { get; set; }
        //public int OrderId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
       // public int CheckoutId { get; set; }
        //public string CustomerId { get; set; }

    }
}
