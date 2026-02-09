using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Auth.Login
{
    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
