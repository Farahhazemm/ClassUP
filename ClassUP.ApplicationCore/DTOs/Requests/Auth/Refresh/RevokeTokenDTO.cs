using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Auth.Refresh
{
    public class RevokeTokenDTO
    {
        // i make it nullable as client maybe send it in cookie 
        public string? Token { get; set; }   
    }
}
