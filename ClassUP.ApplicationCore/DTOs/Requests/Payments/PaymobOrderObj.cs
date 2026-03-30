using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobOrderObj
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("merchant_order_id")]
        public string MerchantOrderId { get; set; } = null!;
    }
}
