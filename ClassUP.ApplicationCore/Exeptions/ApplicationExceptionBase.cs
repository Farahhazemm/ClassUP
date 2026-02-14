using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Exeptions
{
    public class ApplicationExceptionBase : Exception
    {
        protected ApplicationExceptionBase(string message)
           : base(message)
        {
        }
    }
}
