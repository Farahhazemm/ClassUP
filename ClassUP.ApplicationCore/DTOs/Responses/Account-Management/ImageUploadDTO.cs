using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Account_Management
{
    public class ImageUploadDTO
    {
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
    }
}
