using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Auth
{
    public interface IEmailVerificationService
    {
        Task<string> GenerateVerificationCodeAsync(AppUser user);
    }
}
