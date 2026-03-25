using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Payments
{
    using ClassUP.ApplicationCore.DTOs.Requests.Payments;
    using Microsoft.Extensions.Configuration;
    using System.Security.Cryptography;
    using System.Text;

    public class PaymobHmacService
    {
        private readonly string _secret;

        public PaymobHmacService(IConfiguration config)
        {
            _secret = config["Paymob:HmacSecret"];
        }

        public bool IsValid(PaymobWebhookRequestDTO request)
        {
            var obj = request.Obj;

            var data =
                $"{obj.AmountCents}" +
                $"{obj.CreatedAt}" +
                $"{obj.Currency}" +
                $"{obj.ErrorOccured}" +
                $"{obj.HasParentTransaction}" +
                $"{obj.Id}" +
                $"{obj.IntegrationId}" +
                $"{obj.Is3dSecure}" +
                $"{obj.IsAuth}" +
                $"{obj.IsCapture}" +
                $"{obj.IsRefunded}" +
                $"{obj.IsStandalonePayment}" +
                $"{obj.IsVoided}" +
                $"{obj.Order.Id}" +
                $"{obj.Owner}" +
                $"{obj.Pending}" +
                $"{obj.SourceData.Pan}" +
                $"{obj.SourceData.SubType}" +
                $"{obj.SourceData.Type}" +
                $"{obj.Success}";

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

            var calculated = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return calculated == request.Hmac;
        }
    }
}
