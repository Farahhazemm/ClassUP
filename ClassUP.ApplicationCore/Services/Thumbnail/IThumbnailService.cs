using ClassUP.ApplicationCore.DTOs.Cources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Thumbnail
{
    public interface IThumbnailService
    {
        Task<string> SaveAsync(ThumbnailDTO image, string folder);
    }
}
