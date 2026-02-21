using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account.Email
{
    public class ConfirmEmailDTO
    {
        [Required]
        public string Code { get; set; }

        public string UserId { get; set; }  
    }
}
