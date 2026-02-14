using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Exeptions
{
    public class InvalidCredentialsException : ApplicationExceptionBase
    {
        public InvalidCredentialsException() : base("Invalid email or password")
        {
        }
    }
}
