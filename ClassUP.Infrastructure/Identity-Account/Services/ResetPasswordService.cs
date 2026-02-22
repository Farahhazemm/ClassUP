using ClassUP.ApplicationCore.Services.IAccount;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Identity.Services;
using ClassUP.Infrastructure.Identity_Account.Email.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Identity_Account.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<EmailVerificationService> _logger;
        IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        public ResetPasswordService(UserManager<AppUser> userManager,
            ILogger<EmailVerificationService> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env,IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _env= env;
            _emailService = emailService;
        }
        public async Task<string> GenerateResetPasswordCodeAsync(AppUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Generated verification ResetPassword code for user {UserId}: {Code}", user.Id, code);

            return code;

        }

        public async Task SendResetPasswordEmailAsync(AppUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?
                 .Request
                 .Headers
                 .Origin
                 .ToString();

            var confirmationLink = $"{origin}/auth/forgotpassword?email={user.Email}&code={code}";


            var templatePath = Path.Combine(
                _env.WebRootPath,
                "ResetPasswordTemplates",
                "ForgotPassword.html"
            );



            if (!File.Exists(templatePath))
            {

                throw new FileNotFoundException("Email template not found at " + templatePath);
            }

            var emailBody = EmailBodyBuilder.Generate(
                templatePath,
                new Dictionary<string, string>
                {
            { "{{name}}", user.FirstName ?? "User" },
            { "{{action_url}}", confirmationLink }
                }
            );

            await _emailService.SendAsync(
                user.Email!,
                "✅ ClassUP: Email Confirmation",
                emailBody
            );

            _logger.LogInformation("ChangePasswordCode sent to {Email}", user.Email);
        }
    }
}
