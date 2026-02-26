using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account_Management
{
    public class UpdateProfileDTO
    {
        [StringLength(50)] 
        public string? FirstName { get; set; }
        [StringLength(50)] 
        public string? LastName { get; set; }
        [StringLength(1000)]
        public string? Bio { get; set; }
        //TODO : An supporting image Service "Cloudnary" To return this URL
        [Url]
        public string? ProfilePictureUrl { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
