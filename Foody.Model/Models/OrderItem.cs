using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Foody.Model.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        //[ForeignKey("ProductId")]
        public decimal Price { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
    }
}
