using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Videos
{
  
        public interface IVideoService
        {
            Task<string>UploadAsync(IFormFile videoFile);
             Task<bool>DeleteAsync(string publicId);
    }
    }

