using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account.Password
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
