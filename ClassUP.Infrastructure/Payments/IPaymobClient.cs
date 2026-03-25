using ClassUP.ApplicationCore.DTOs.Responses.Payments;
using Refit;

namespace ClassUP.Infrastructure.Payments
{
    public interface IPaymobClient
    {
        [Post("/api/auth/tokens")]
        Task<AuthResponse> GetAuthToken([Body] object body);

        [Post("/api/ecommerce/orders")]
        Task<OrderResponse> CreateOrder([Body] object body);

        [Post("/api/acceptance/payment_keys")]
        Task<PaymentKeyResponse> CreatePaymentKey([Body] object body);
    }
}
