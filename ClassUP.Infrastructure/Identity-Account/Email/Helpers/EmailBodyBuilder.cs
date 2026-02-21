using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Identity_Account.Email.Helpers
{
    public static class EmailBodyBuilder
    {
        public static string Generate(
            string templateFullPath,
            Dictionary<string, string> placeholders)
        {
            if (!File.Exists(templateFullPath))
                throw new FileNotFoundException($"Email template not found: {templateFullPath}");

            var body = File.ReadAllText(templateFullPath, Encoding.UTF8);

            foreach (var placeholder in placeholders)
            {
                body = body.Replace(placeholder.Key, placeholder.Value);
            }

            return body;
        }
    }
}
