using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.ForgotPassword
{
    public class VerifyForgotPasswordResponseDTO
    {
        public string ResetToken { get; set; } = null!;
    }
}
