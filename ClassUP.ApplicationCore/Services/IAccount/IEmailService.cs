using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IAccount
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string htmlBody);
    }
}
