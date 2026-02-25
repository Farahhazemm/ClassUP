using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.User
{
     public class UpdateUserDTO
    {
        
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string ?FirstName { get; set; } = null!;

     
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string? LastName { get; set; } = null!;

     
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string ?Email { get; set; } = null!;

      
       
        public IList<string>? Roles { get; set; } = [];
    }
}
