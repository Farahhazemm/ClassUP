using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Register;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.Domain.Constants;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserTokenService _tokenService;
        private readonly IEmailVerificationService _emailVerificationService;
        public AuthService(UserManager<AppUser> userManager, IUserTokenService tokenService , IEmailVerificationService emailVerificationService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailVerificationService = emailVerificationService;
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO dto)
        {
            //  Create user object
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            //  Create user in Identity
            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                throw new IdentityOperationException(createResult.Errors.Select(e => e.Description));

            //  Assign default role
            var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new IdentityOperationException(roleResult.Errors.Select(e => e.Description));
            }

            //  Generate email verification code
            var verificationCode = await _emailVerificationService.GenerateVerificationCodeAsync(user);

            //  Send verification email
          //  await _emailVerificationService.SendVerificationEmailAsync(user, verificationCode);

            //  Get roles => for return in DTO
            var roles = await _userManager.GetRolesAsync(user);

            //  Return res
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

            if (!user.EmailConfirmed)
                throw new EmailNotConfirmedException();


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

        public async Task ConfirmEmailAsync(ConfirmEmailDTO request )
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new InvalidCodeException("Invalid verification code or user does not exist.");

            if (user.EmailConfirmed)
                throw new InvalidCodeException("Email already confirmed.");

            string code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            }
            catch (FormatException)
            {
                throw new InvalidCodeException("Invalid verification code format.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                throw new IdentityOperationException(new[] { error.Description });
            }

            
           // _logger.LogInformation("User {UserId} confirmed email successfully.", user.Id);
        }


public async Task ResendConfirmationEmailAsync(ResendConfirmationEmailDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        
        if (user == null)
            return; 

        if (user.EmailConfirmed)
            throw new BadRequestException("Email is already confirmed.");

        var verificationCode =
            await _emailVerificationService.GenerateVerificationCodeAsync(user);

        // TODO:
        // await _emailVerificationService.SendVerificationEmailAsync(user, verificationCode);
    }


}
}
