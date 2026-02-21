using ClassUP.ApplicationCore.Services.IAccount;
using ClassUP.Infrastructure.Identity_Account.Email.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Identity_Account.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<MailSettings> mailSettings,
            ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = subject
            };

            message.To.Add(MailboxAddress.Parse(toEmail));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            _logger.LogInformation(
                "Sending email to {Email} using host {Host}",
                toEmail,
                _mailSettings.Host
            );

            await smtp.ConnectAsync(
                _mailSettings.Host,
                _mailSettings.Port,
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _mailSettings.Mail,
                _mailSettings.Password
            );

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
        }
    }
}
