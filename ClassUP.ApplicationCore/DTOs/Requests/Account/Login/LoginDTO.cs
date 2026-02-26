using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Auth.Login
{
        public class LoginDTO
        {
            [Required, EmailAddress, MaxLength(100)]
            public string Email { get; set; } = null!;

            [Required, MinLength(8), MaxLength(100)]
            public string Password { get; set; } = null!;
        }
    
}
