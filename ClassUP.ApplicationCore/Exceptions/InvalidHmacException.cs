using ClassUP.ApplicationCore.Exeptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ClassUP.ApplicationCore.Exceptions
{
    public class InvalidHmacException : AppException
    {
        public InvalidHmacException()
            : base("Invalid HMAC signature.", HttpStatusCode.Unauthorized)
        {
        }
    }
}
