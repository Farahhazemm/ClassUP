using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public AppUser User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
    }
}
