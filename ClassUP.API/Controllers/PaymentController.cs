using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Payments;
using ClassUP.ApplicationCore.IServices.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("create/{courseId}")]
        public async Task<IActionResult> Create(int courseId)
        {
            var userId = User.GetUserId();
            var result = await _paymentService.CreatePaymentAsync(courseId, userId);
            return Ok(result);
        }


        [HttpGet("webhook")]
        [AllowAnonymous]
        public IActionResult WebhookGet()
        {
            return Ok(); // Paymob pings this to verify the URL
        }


        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook([FromQuery] string hmac)
        {
            if (string.IsNullOrEmpty(hmac))
                return BadRequest("Missing HMAC.");

            using var reader = new StreamReader(Request.Body);
            var rawBody = await reader.ReadToEndAsync();

            var webhookData = JsonSerializer.Deserialize<PaymobWebhookRequestDTO>(rawBody);

            if (webhookData?.obj == null)
                return BadRequest("Invalid webhook body.");

            webhookData.hmac = hmac;
            await _paymentService.HandleWebhookAsync(webhookData);
            return Ok();
        }
    }
}