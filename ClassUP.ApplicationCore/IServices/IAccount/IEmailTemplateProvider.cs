using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.IAccount
{
    public interface IEmailTemplateProvider
    {
        // in html form
        string GetEmailConfirmationTemplate(string code);
    }
}
