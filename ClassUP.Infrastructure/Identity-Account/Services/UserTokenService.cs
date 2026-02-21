using ClassUP.ApplicationCore.DTOs.Responses.Auth.Refresh;
using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
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
                // Identity Claims
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));

            // JWT Standard Claims
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

        #region createAnRefreshToken
        public async Task<TokensDTO> RefreshTokenAsync(string Token )
        {
            var tokens = new TokensDTO();

            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == Token));



            if (user == null)
                    throw new SecurityException("Invalid refresh token");
            
            // I Use single => before chech confirm that The curr user has refreshtoken
            var refreshtoken = user.RefreshTokens.Single(t => t.Token == Token);
            if (!refreshtoken.IsActive)
            {
                throw new SecurityException("Invalid refresh token");
            }

            //  arrive here => i sure that my end user send a valid refreshToken & still actiive 
            // now I make a three steps 
            //1 : Revoke an old token 
            //2 : generate a new refresh token 
            //3 : Generate a new JWT TOken 
      
            refreshtoken.RevokedOn = DateTime.UtcNow;

            var newrefreshtoken = GenerateRefreshToken(user.Id);
            user.RefreshTokens.Add(newrefreshtoken);

            await _userManager.UpdateAsync(user);

            var jwttoken = await GenerateJwtAsync(user);
            tokens.JwtToken = jwttoken.Token;
            tokens.RefreshToken = newrefreshtoken.Token;
            tokens.RefreshTokenExpiration = newrefreshtoken.ExpiresOn;

            return tokens;


        }
        #endregion


        #region RevokecurrToken
        public async Task<bool> RevokeTokenAsync(string Token)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == Token));
            if (user == null)
                return false;
            var refreshtoken = user.RefreshTokens.Single(t => t.Token == Token);
            if (!refreshtoken.IsActive)
                return false;


            refreshtoken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return true;
        }

        #endregion

        #region RevokeAllForLogout
        public async Task RevokeAllAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return;

            var activeTokens = user.RefreshTokens
                .Where(t => t.IsActive)
                .ToList();

            foreach (var token in activeTokens)
            {
                token.RevokedOn = DateTime.UtcNow;
            }

            await _userManager.UpdateAsync(user);
        }


        #endregion

        #region GenerateRefreshToken
        public RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[32];

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,
                UserId = userId
            };
        }

        #endregion


    }
}
