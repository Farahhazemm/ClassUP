using ClassUP.Domain.Models;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.IIdentity
{
    public interface IUserTokenService
    {
        Task<(string Token, DateTime Expiration)> GenerateJwtAsync(AppUser user);
    }
}
