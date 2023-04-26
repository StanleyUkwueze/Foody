using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Foody.Model.Models
{
    public class Order
    {
        //public enum OrderStatus
        //{
        //    Placed,
        //    Shipped,
        //    Delivered,
        //    Cancelled
        //}
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public int CheckOutId { get; set; }
        public decimal TotalPrice { get; set; }
        public int AddressId { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCancelled { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime Shipped { get; set; }
        //public OrderStatus Status { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [JsonIgnore]
        public Address ShippingAddress { get; set; }
        public Customer Customer { get; set; }
        [JsonIgnore]
        [ForeignKey("CheckOutId")]
        public CheckOut CheckOut { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
