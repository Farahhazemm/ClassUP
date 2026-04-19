using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IImage
{
    public interface IImageValidator
    {
        void Validate(IFormFile file);
    }
}
