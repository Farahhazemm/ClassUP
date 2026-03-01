using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Account_Management
{
    public class UpdateProfileImageDTO
    {
        public IFormFile Image { get; set; } = null!;
    }
}
