using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IAccount
{
    public  interface IResetPasswordService
    {
      Task<string> GenerateResetPasswordCodeAsync(AppUser user);
      Task SendResetPasswordEmailAsync(AppUser user, string code);
    }
}
