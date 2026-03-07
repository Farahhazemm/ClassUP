using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Videos
{
  
        public interface IVideoService
        {
        Task<VideoResult> UploadAsync(IFormFile file);
        Task DeleteAsync(string publicId);
         }
 }

