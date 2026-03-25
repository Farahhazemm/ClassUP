using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Payments;
using ClassUP.ApplicationCore.IServices.Payments;
using ClassUP.ApplicationCore.Services.Cart;
using ClassUP.ApplicationCore.Services.Enrollment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create/{courseId}")]
        public async Task<IActionResult> Create(int courseId)
        {
            var userId = User.GetUserId();

            var result = await _paymentService.CreatePaymentAsync(courseId, userId);

            return Ok(result);
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook([FromBody] PaymobWebhookRequestDTO request)
        {
            await _paymentService.HandleWebhookAsync(request);
            return Ok();
        }


    }
}
