using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Register;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.Domain.Constants;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ClassUP.ApplicationCore.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserTokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, IUserTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                throw new IdentityOperationException(createResult.Errors.Select(e => e.Description));

            var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new IdentityOperationException(roleResult.Errors.Select(e => e.Description));
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                Roles = roles
            };
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
           
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new InvalidCredentialsException();

        
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                throw new InvalidCredentialsException();

         
            var (jwtToken, jwtExpiration) = await _tokenService.GenerateJwtAsync(user);

            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new LoginResponseDTO
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresOn
            };
        }


    }
}
