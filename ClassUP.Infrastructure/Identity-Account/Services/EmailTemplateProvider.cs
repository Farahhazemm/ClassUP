using ClassUP.ApplicationCore.Services.IAccount;
using System;

namespace ClassUP.Infrastructure.Identity_Account.Services
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        public string GetEmailConfirmationTemplate(string code)
        {
            // Return the email body as HTML content
            return $@"
                <h2>Confirm Your Email</h2>
                <p>Please use the following code to confirm your email address:</p>
                <h1>{code}</h1>
                <p>This code is valid for 10 minutes.</p>
            ";
        }
    }
}
