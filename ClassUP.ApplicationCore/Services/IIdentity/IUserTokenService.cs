using ClassUP.ApplicationCore.DTOs.Responses.Auth.Refresh;
using ClassUP.Domain.Models;
using System.Threading.Tasks;

namespace ClassUP.ApplicationCore.Services.IIdentity
{
    public interface IUserTokenService
    {
        Task<(string Token, DateTime Expiration)> GenerateJwtAsync(AppUser user);
        RefreshToken GenerateRefreshToken(string userId);
        Task<TokensDTO> RefreshTokenAsync(string Token );
    }
}
