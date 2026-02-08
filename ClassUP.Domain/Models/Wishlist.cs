using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime AddedAt { get; set; }
        public AppUser User { get; set; }
        public Course Course { get; set; }
    }
}
