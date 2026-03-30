//using System.Security.Cryptography;
//using System.Text;
//using ClassUP.ApplicationCore.DTOs.Requests.Payments;
//using Microsoft.Extensions.Configuration;

//namespace ClassUP.Infrastructure.Payments
//{
//    public class PaymobHmacService
//    {
//        private readonly string _secret;

//        public PaymobHmacService(IConfiguration config)
//        {
//            _secret = config["Paymob:HmacSecret"]
//                ?? throw new InvalidOperationException(
//                    "Paymob:HmacSecret is not configured in appsettings.");
//        }

//        public bool IsValid(PaymobWebhookRequestDTO request)
//        {
//            var obj = request.obj;

//            // Booleans => be toLower (true/false) to match Paymob output
//            var data =
//                $"{obj.AmountCents}" +
//                $"{obj.CreatedAt}" +
//                $"{obj.Currency}" +
//                $"{obj.ErrorOccured.ToString().ToLower()}" +
//                $"{obj.HasParentTransaction.ToString().ToLower()}" +
//                $"{obj.Id}" +
//                $"{obj.IntegrationId}" +
//                $"{obj.Is3dSecure.ToString().ToLower()}" +
//                $"{obj.IsAuth.ToString().ToLower()}" +
//                $"{obj.IsCapture.ToString().ToLower()}" +
//                $"{obj.IsRefunded.ToString().ToLower()}" +
//                $"{obj.IsStandalonePayment.ToString().ToLower()}" +
//                $"{obj.IsVoided.ToString().ToLower()}" +
//                $"{obj.Order.Id}" +
//                $"{obj.Owner}" +
//                $"{obj.Pending.ToString().ToLower()}" +
//                $"{obj.SourceData.Pan}" +
//                $"{obj.SourceData.SubType}" +
//                $"{obj.SourceData.Type}" +
//                $"{obj.Success.ToString().ToLower()}";

//            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_secret));
//            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
//            var calculated = BitConverter.ToString(hash).Replace("-", "").ToLower();

//            //  constanttime  to prevent timing attack
//            return CryptographicOperations.FixedTimeEquals(
//                Encoding.UTF8.GetBytes(calculated),
//                Encoding.UTF8.GetBytes(request.hmac ?? string.Empty));
//        }
//    }
//}