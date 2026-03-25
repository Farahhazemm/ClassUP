using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Payments
{
    public class PaymentResponseDTO
    {
        public bool IsFreeCourse { get; set; }

        public string? PaymentUrl { get; set; }

        public int? OrderId { get; set; }
    }
}
