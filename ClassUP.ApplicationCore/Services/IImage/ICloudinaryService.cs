using ClassUP.ApplicationCore.DTOs.Responses.Account_Management;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IImage
{
    public interface ICloudinaryService
    {
        Task<ImageUploadDTO> UploadAsync(IFormFile file, string folder);
        Task DeleteAsync(string publicId);

    }
}
