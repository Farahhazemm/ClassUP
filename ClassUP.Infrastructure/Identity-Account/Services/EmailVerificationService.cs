using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.Domain.Models;
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
        private readonly IEmailSender _emailSender;

        public EmailVerificationService(
            UserManager<AppUser> userManager,
           // IEmailSender emailSender,
            ILogger<EmailVerificationService> logger)
        {
            _userManager = userManager;
           // _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<string> GenerateVerificationCodeAsync(AppUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Generated verification code for user {UserId}: {Code}", user.Id, code);

            return code;
        }


    }
}
