using ClassUP.ApplicationCore.DTOs.Responses.Account_Management;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Helpers.Cloudniary;
using ClassUP.ApplicationCore.Services.IImage;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ClassUP.Infrastructure.Services.Images
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        #region Upload
        public async Task<ImageUploadDTO> UploadAsync(IFormFile file, string folder)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,  //users/{userId} Or courses/{courseId}
                Transformation = new Transformation()
                    .Width(800) 
                    .Height(800)
                    .Crop("limit") 
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Created)
                throw new BadRequestException("Failed to upload image");

            return new ImageUploadDTO
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            await _cloudinary.DestroyAsync(new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            });
        } 
        #endregion
    }
}

