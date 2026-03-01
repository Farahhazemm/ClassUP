using ClassUP.ApplicationCore.DTOs.Responses.Account_Management;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IImage
{
    public interface ICloudinaryService
    {
        Task<ImageUploadDTO> UploadProfileImageAsync(IFormFile file, string userId);
        Task DeleteAsync(string publicId);

    }
}
