using ClassUP.ApplicationCore.DTOs.Requests.Payments;
using ClassUP.ApplicationCore.DTOs.Responses.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IServices.Payments
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO> CreatePaymentAsync(int courseId, string userId);
        Task HandleWebhookAsync(PaymobWebhookRequestDTO request);
    }
}
