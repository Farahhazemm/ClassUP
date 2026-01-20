using ClassUP.ApplicationCore.DTOs.Cources;
using System.IO;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.Thumbnail
{
    public interface IThumbnailService
    {
        Task<string> SaveAsync(ThumbnailDTO image, string folder);

        Task DeleteAsync(string imageUrl);
    }

}
