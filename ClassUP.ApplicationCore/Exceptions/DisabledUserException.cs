using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ClassUP.ApplicationCore.Exeptions
{
    public class DisabledUserException : AppException
    {
        public DisabledUserException()
            : base("User account is disabled.", HttpStatusCode.Forbidden)
        {
        }
    }
}