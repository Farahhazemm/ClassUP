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
            _logger.LogInformation("Paymob GET webhook ping received.");
            return Ok(); // Paymob pings this to verify the URL
        }

        //[HttpPost("webhook")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Webhook(
        //    [FromBody] PaymobWebhookRequestDTO request,
        //    [FromQuery] string hmac)
        //{
        //    _logger.LogInformation("Webhook hit. HMAC: '{Hmac}'", hmac ?? "NULL");
        //    _logger.LogInformation("request null? {IsNull}", request == null);
        //    _logger.LogInformation("request.obj null? {IsNull}", request?.obj == null);

        //    if (string.IsNullOrEmpty(hmac))
        //    {
        //        _logger.LogWarning("Missing HMAC query parameter.");
        //        return BadRequest("Missing HMAC.");
        //    }

        //    try
        //    {
        //        request!.hmac = hmac;
        //        await _paymentService.HandleWebhookAsync(request);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("WEBHOOK FAILED: {Type} — {Message}", ex.GetType().Name, ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("webhook/raw")]
        //[AllowAnonymous]
        //public async Task<IActionResult> WebhookRaw()
        //{
        //    using var reader = new StreamReader(Request.Body);
        //    var raw = await reader.ReadToEndAsync();
        //    _logger.LogInformation("RAW WEBHOOK BODY: {Body}", raw);
        //    return Ok();
        //}

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook([FromQuery] string hmac)
        {
            if (string.IsNullOrEmpty(hmac))
                return BadRequest("Missing HMAC.");

            using var reader = new StreamReader(Request.Body);
            var rawBody = await reader.ReadToEndAsync();

            // LOG THE RAW BODY
            _logger.LogInformation("RAW WEBHOOK: {Body}", rawBody);

            var webhookData = JsonSerializer.Deserialize<PaymobWebhookRequestDTO>(rawBody);

            // LOG WHAT WAS PARSED
            _logger.LogInformation("PARSED: obj null={IsNull}, Success={S}, Pending={P}",
                webhookData?.obj == null,
                webhookData?.obj?.Success,
                webhookData?.obj?.Pending);

            if (webhookData?.obj == null)
                return BadRequest("Invalid webhook body.");

            webhookData.hmac = hmac;
            await _paymentService.HandleWebhookAsync(webhookData);
            return Ok();
        }
    }
}