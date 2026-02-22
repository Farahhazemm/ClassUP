using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.ApplicationCore.Services.IAccount;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Identity_Account.Email.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Identity.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<EmailVerificationService> _logger;
        private readonly IEmailService _emailService;
        IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public EmailVerificationService(
            UserManager<AppUser> userManager,
            IEmailService emailService,
            ILogger<EmailVerificationService> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public async Task<string> GenerateVerificationCodeAsync(AppUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Generated verification code for user {UserId}: {Code}", user.Id, code);

            return code;
        }

        public async Task SendConfirmationEmailAsync(AppUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?
                .Request
                .Headers
                .Origin
                .ToString();

            var confirmationLink = $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}";

          
            var templatePath = Path.Combine(
                _env.WebRootPath,              
                "EmailTemplates",              
                "EmailConfirmation.html"
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

            _logger.LogInformation("Confirmation email sent to {Email}", user.Email);
        }
    }
}
