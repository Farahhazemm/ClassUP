using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobSourceData
    {
        public string Pan { get; set; } = null!;
        public string SubType { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

}
