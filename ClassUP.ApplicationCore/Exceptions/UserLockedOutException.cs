using ClassUP.ApplicationCore.Exeptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public class UserLockedOutException : AppException
{
    public UserLockedOutException()
        : base("User account is temporarily locked due to multiple failed login attempts.",
               HttpStatusCode.Forbidden)
    {
    }
}