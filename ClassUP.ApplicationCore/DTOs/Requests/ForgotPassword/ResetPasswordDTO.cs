using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.ForgotPassword
{
    public class ResetPasswordDTO
    {
        public string ResetToken { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
