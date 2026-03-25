using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobWebhookRequestDTO
    {
        public string Hmac { get; set; }
        public PaymobWebhookObj Obj { get; set; }
    }

}
