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
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppUser User { get; set; }
        public Order Order { get; set; }
    }
}
