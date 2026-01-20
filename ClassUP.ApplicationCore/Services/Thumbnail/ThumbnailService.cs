using ClassUP.ApplicationCore.DTOs.Cources;

namespace ClassUP.ApplicationCore.Services.Thumbnail
{
    public class ThumbnailService : IThumbnailService
    {
        
            public async Task<string> SaveAsync(ThumbnailDTO image, string folder)
            {
                var uploadsPath = Path.Combine("wwwroot", folder);

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                // get extension from mime type
                var extension = image.MimeType switch
                {
                    "image/jpeg" => ".jpg",
                    "image/png" => ".png",
                    "image/webp" => ".webp",
                    _ => throw new InvalidOperationException("Unsupported image type")
                };

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.FileStream.CopyToAsync(stream);
                }

                return $"/{folder}/{fileName}";
            }

            public Task DeleteAsync(string imageUrl)
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return Task.CompletedTask;

                // Remove leading slash if exists
                var relativePath = imageUrl.TrimStart('/');

                var fullPath = Path.Combine("wwwroot", relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                return Task.CompletedTask;
            }
        }
    }

