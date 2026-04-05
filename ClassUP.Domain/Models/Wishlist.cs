using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Wishlist : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public int CourseId { get; set; }
        //public DateTime AddedAt { get; set; }
        public AppUser User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
