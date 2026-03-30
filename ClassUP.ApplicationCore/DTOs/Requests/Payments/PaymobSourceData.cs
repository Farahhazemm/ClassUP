using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobSourceData
    {
        [JsonPropertyName("pan")]
        public string Pan { get; set; } = null!;

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }

}
