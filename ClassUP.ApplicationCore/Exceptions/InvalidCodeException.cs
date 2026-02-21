using ClassUP.ApplicationCore.Exeptions;
using System.Net;

namespace ClassUP.ApplicationCore.Exceptions
{
    public class InvalidCodeException : AppException
    {
        public InvalidCodeException(string message)
            : base(message, HttpStatusCode.BadRequest) 
        {
        }
    }
}