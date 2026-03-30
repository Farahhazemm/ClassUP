using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobWebhookRequestDTO
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("obj")]
        public PaymobTransactionObj obj { get; set; } = null!;

        public string? hmac { get; set; }
    }

}
