using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Model.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public int CheckOutId { get; set; }
        public int AddressId { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus OrderStatus { get; set; }
        public Address ShippingAddress { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("CheckOutId")]
        public CheckOut CheckOut { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
