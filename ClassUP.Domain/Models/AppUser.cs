using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ClassUP.Domain.Models
{
    public class AppUser : IdentityUser
    {
        #region My Prop
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }

        #endregion

        #region Busniss navigation 
        public List<Course> Courses { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; } 
        #endregion

        public List <RefreshToken>? RefreshTokens { get; set; }
    }
}
