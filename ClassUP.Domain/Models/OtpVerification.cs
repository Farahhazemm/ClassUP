using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Domain.Models
{

    public class OtpVerification
    {
       
        public string Code { get; set; } = null!;

        
        public int RemainingAttempts { get; set; }
    }
}
