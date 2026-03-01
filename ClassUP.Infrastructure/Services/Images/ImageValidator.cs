using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Helpers.Images;
using ClassUP.ApplicationCore.Services.IImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClassUP.Infrastructure.Services.Images
{
    public class ImageValidator : IImageValidator
    {
        // White listing => Content Validation
        

        public void Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("Image is required");

            if (file.Length > ImageSettings.MaxFileSizeInBytes)
                throw new BadRequestException(
                    $"Image size must be less than {ImageSettings.MaxFileSizeInMB}MB");

            // avoid new folder generate 
            if (file.FileName.Contains("/") ||file.FileName.Contains("\\") || file.FileName.Contains(".."))
            {
                throw new BadRequestException("Invalid file name");
            }

            if (!ImageSettings.AllowedMimeTypes.Contains(file.ContentType))
                throw new BadRequestException("Unsupported image type");

            // Binary sig
            using var reader = new BinaryReader(file.OpenReadStream());
            var bytes = reader.ReadBytes(2);
            var fileHex = BitConverter.ToString(bytes);

            if (!ImageSettings.FileSignatures.TryGetValue(file.ContentType, out var signatures) ||
                !signatures.Contains(fileHex))
            {
                throw new BadRequestException("Invalid image content");
            }

        }
    }
}
