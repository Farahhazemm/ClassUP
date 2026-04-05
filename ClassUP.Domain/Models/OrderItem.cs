using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Order Order { get; set; } = null!;

    }
}
