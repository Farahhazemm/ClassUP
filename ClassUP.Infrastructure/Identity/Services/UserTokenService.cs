using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClassUP.Infrastructure.Identity.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserTokenService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        #region JWT_Token
        public async Task<(string Token, DateTime Expiration)> GenerateJwtAsync(AppUser user)
        {
            
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                userClaims.Add(new Claim(ClaimTypes.Role, role));

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var lifetimeMinutes = Convert.ToDouble(_configuration["JWT:Lifetime"]);
            var expiration = DateTime.UtcNow.AddMinutes(lifetimeMinutes);

       
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: userClaims,
                expires: expiration,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, expiration);
        }

       
        #endregion
    }
}
