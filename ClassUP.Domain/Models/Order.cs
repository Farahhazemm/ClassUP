using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }

        // Navigation properties
        public AppUser User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = [];
        public Payment Payment { get; set; } = null!;
    }
}
