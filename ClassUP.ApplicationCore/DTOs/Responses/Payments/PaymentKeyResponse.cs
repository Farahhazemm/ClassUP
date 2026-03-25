using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Responses.Payments
{
    public class PaymentKeyResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;
    }
}
