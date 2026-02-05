using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.ApplicationCore.Services.Videos;

namespace ClassUP.Infrastructure.ExternalServices
{
    public class VideoService : IVideoService
    {
        private readonly Cloudinary _cloudinary;

        public VideoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
            


        }

        #region Upload
        public async Task<VideoResult> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
                EagerTransforms = new List<Transformation>
                {
                  new EagerTransformation().Width(720).Height(480).Crop("fit"),
                  new EagerTransformation().Width(160).Height(90).Crop("fill").AudioCodec("none")
                },
                EagerAsync = true,

            };


            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != HttpStatusCode.OK)
                throw new Exception("Video upload failed.");

            return new VideoResult
            {
                VideoUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                throw new ArgumentException("PublicId is required.");

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception("Video deletion failed.");
        }
        #endregion
    }
}
