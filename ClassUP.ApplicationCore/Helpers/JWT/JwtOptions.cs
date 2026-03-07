using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Helpers.JWT
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public string SigningKey { get; set; }
    }
}
