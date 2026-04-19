using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Payments;
using ClassUP.ApplicationCore.IServices.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;

namespace ClassUP.API.Controllers
{
    /// <summary>
    /// Handles payment operations including creating payments and processing Paymob webhooks.
    /// </summary>
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
        /// <summary>
        /// Creates a payment for a specific course.
        /// If the course is free, the user will be enrolled directly.
        /// Otherwise, a Paymob payment URL will be returned.
        /// </summary>
        /// <response code="200">Payment created successfully or free enrollment completed.</response>
        /// <response code="400">Invalid request or user already enrolled.</response>
        /// <response code="404">Course not found.</response>

        [HttpPost("create/{courseId}")]
        [EnableRateLimiting("userlimit")]

        public async Task<IActionResult> Create(int courseId)
        {
            var userId = User.GetUserId();
            var result = await _paymentService.CreatePaymentAsync(courseId, userId);
            return Ok(result);
        }

        /// <summary>
        /// Paymob webhook verification endpoint (GET).
        /// Used by Paymob to verify that the webhook URL is active.
        /// </summary>
        /// <response code="200">Webhook endpoint is active.</response>
        [HttpGet("webhook")]
        [EnableRateLimiting("iplimit")]
        [AllowAnonymous]
        public IActionResult WebhookGet()
        {
            return Ok(); // Paymob pings this to verify the URL
        }

        /// <summary>
        /// Handles Paymob payment webhook notifications.
        /// Processes payment success/failure and updates orders and enrollments.
        /// </summary>
        /// <response code="200">Webhook processed successfully.</response>
        /// <response code="400">Invalid webhook payload or missing HMAC.</response>

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