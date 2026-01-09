using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        

        public List<Course> Courses { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
