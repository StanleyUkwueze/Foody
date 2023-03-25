using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Model.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int CheckoutId { get; set; }
        public string CustomerId { get; set; }
       // [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
       // [ForeignKey("CheckoutId")]
        public List<Order> Orders { get; set; }


    }
}
