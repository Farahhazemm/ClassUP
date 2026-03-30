using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobTransactionObj
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("pending")]
        public bool Pending { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("error_occured")]
        public bool ErrorOccured { get; set; }

        [JsonPropertyName("order")]
        public PaymobOrderObj Order { get; set; } = null!;
    }
}
