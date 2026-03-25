using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Payments
{
    public class PaymobSettings
    {
        public string ApiKey { get; set; } = null!;
        public int IntegrationId { get; set; }
        public int IframeId { get; set; }
        public string HmacSecret { get; set; } = null!;
    }
}
