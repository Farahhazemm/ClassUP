using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ClassUP.ApplicationCore.Exeptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        protected AppException(string message , HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
           : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
