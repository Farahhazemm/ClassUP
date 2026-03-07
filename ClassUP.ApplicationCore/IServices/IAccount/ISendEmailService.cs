using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IAccount
{
    public interface ISendEmailService
    {
        Task SendEmailConfirmationAsync(string email, string code);
    }
}
