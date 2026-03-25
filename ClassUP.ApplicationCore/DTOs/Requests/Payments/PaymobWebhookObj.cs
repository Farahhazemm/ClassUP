using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Payments
{
    public class PaymobWebhookObj
    {
        public string Id { get; set; } = null!;
        public int AmountCents { get; set; }
        public bool Success { get; set; }

        public string CreatedAt { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public bool ErrorOccured { get; set; }
        public bool HasParentTransaction { get; set; }
        public int IntegrationId { get; set; }
        public bool Is3dSecure { get; set; }
        public bool IsAuth { get; set; }
        public bool IsCapture { get; set; }
        public bool IsRefunded { get; set; }
        public bool IsStandalonePayment { get; set; }
        public bool IsVoided { get; set; }
        public string Owner { get; set; } = null!;
        public bool Pending { get; set; }

        public PaymobSourceData SourceData { get; set; } = null!;
        public PaymobOrder Order { get; set; } = null!;
    }
}
