using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ClassUP.ApplicationCore.Exeptions
{
    // sealed => Can't inhert from it 
    public sealed class NotFoundException : AppException
    {
        public NotFoundException(string resourceName)
            : base($"{resourceName} not found.", HttpStatusCode.NotFound)
        {
        }
    }
}
