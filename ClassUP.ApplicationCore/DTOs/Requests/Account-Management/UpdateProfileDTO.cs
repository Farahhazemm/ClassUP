using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account_Management
{
    public class UpdateProfileDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Bio { get; set; }

        // Get An URL From Cloudinary
        public string? ProfilePictureUrl { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
