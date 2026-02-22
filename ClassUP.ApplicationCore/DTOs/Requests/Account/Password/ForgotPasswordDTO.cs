using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account_Management
{
    public class ForgotPasswordDTO
    {
        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; } = null!;
    }
}
