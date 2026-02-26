using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account.Email
{
    public class ResendConfirmationEmailDTO
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;
    }
}
