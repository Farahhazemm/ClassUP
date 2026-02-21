using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ClassUP.ApplicationCore.Exeptions
{
   
        public sealed class EmailNotConfirmedException : AppException
        {
            public EmailNotConfirmedException()
                : base("Email is not confirmed", HttpStatusCode.Forbidden)
            {
            }
        }
    
}
