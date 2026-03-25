using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public AppUser User { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}
