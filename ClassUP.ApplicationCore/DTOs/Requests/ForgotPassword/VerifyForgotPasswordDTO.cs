using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.ForgotPassword
{
    public class VerifyForgotPasswordDTO
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
    }

}
