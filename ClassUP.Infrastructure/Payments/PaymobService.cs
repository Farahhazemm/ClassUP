using ClassUP.ApplicationCore.DTOs.Requests.Payments;
using ClassUP.ApplicationCore.DTOs.Responses.Payments;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.IServices.Payments;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClassUP.Infrastructure.Payments
{
    public class PaymobService : IPaymentService
    {
        private readonly IPaymobClient _client;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaymobSettings _settings;
        //private readonly PaymobHmacService _hmac;
        private readonly ILogger<PaymobService> _logger;
        private readonly UserManager<AppUser> _userManager;

        public PaymobService(
            IPaymobClient client,
            IUnitOfWork unitOfWork,
            IOptions<PaymobSettings> settings,
            //PaymobHmacService hmac,
            ILogger<PaymobService> logger,
            UserManager<AppUser> userManager)
        {
            _client = client;
            _unitOfWork = unitOfWork;
            _settings = settings.Value;
           // _hmac = hmac;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<PaymentResponseDTO> CreatePaymentAsync(int courseId, string userId)
        {
            // Validate course exists
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException("Course");

            // already enrolled 
            var alreadyEnrolled = await _unitOfWork.Enrollments
                .ExistsAsync(e => e.CourseId == courseId && e.UserId == userId);

            if (alreadyEnrolled)
                throw new BadRequestException("User is already enrolled in this course.");

            // Free course flow
            if (course.Price == 0)
            {
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    UserId = userId,
                    EnrolledAt = DateTime.UtcNow,
                    ProgressPercentage = 0
                };

                await _unitOfWork.Enrollments.AddAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();

                return new PaymentResponseDTO { IsFreeCourse = true };
            }

            //  Guard: no duplicate pending order
            var hasPendingOrder = await _unitOfWork.Orders
                .ExistsAsync(o =>
                    o.UserId == userId &&
                    o.OrderItems.Any(i => i.CourseId == courseId) &&
                    o.Status == OrderStatus.Pending);

            if (hasPendingOrder)
                throw new BadRequestException("A pending payment already exists for this course.");

            // Fetch real user data 
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User");

            // Create DB order 
            var orderEntity = new Order
            {
                UserId = userId,
                Total = course.Price,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        CourseId = courseId,
                        CourseTitle = course.Title,
                        Price = course.Price
                    }
                }
            };

            await _unitOfWork.Orders.AddAsync(orderEntity);
            await _unitOfWork.SaveChangesAsync();

            // Call Paymob — rollback DB order on any failure 
            try
            {
                // Auth token
                var auth = await _client.GetAuthToken(new
                {
                    api_key = _settings.ApiKey
                });

                // Create Paymob order
                var paymobOrder = await _client.CreateOrder(new
                {
                    auth_token = auth.Token,
                    delivery_needed = "false",
                    amount_cents = (int)(course.Price * 100),
                    merchant_order_id = orderEntity.Id.ToString()
                });

                // Store Paymob Order ID for reconciliation / refunds
              //  orderEntity.PaymobOrderId = paymobOrder.Id.ToString();
               // await _unitOfWork.SaveChangesAsync();

                // Payment key
                var firstName = user.FirstName;
                var lastName = user.LastName;

                var paymentKey = await _client.CreatePaymentKey(new
                {
                    auth_token = auth.Token,
                    amount_cents = (int)(course.Price * 100),
                    order_id = paymobOrder.Id,
                    currency = "EGP",
                    integration_id = _settings.IntegrationId,
                    billing_data = new
                    {
                        email = user.Email ?? "NA",
                        first_name = firstName,
                        last_name = lastName,
                        phone_number = user.PhoneNumber ?? "NA",
                        apartment = "NA",
                        floor = "NA",
                        street = "NA",
                        building = "NA",
                        shipping_method = "NA",
                        postal_code = "NA",
                        city = "Cairo",
                        country = "EG",
                        state = "Cairo"
                    }
                });

                // Return payment URL
                return new PaymentResponseDTO
                {
                    OrderId = orderEntity.Id,
                    PaymentUrl =
                        $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentKey.Token}"
                };
            }
            catch (Exception ex)
            {
                // Paymob call failed — cancel the ghost order so it doesn't pollute the DB
                _logger.LogError(ex, "Paymob API call failed for Order {OrderId}. Cancelling order.", orderEntity.Id);
                orderEntity.Status = OrderStatus.Cancelled;
                await _unitOfWork.SaveChangesAsync();
                throw;
            }
        }

        public async Task HandleWebhookAsync(PaymobWebhookRequestDTO request)
        {
            // ── 1. Validate HMAC (security — never skip in production) ───────────────
          /*  if (!_hmac.IsValid(request))
            {
                _logger.LogWarning("Invalid HMAC detected. Webhook rejected.");
                throw new InvalidHmacException();
            }*/



            var obj = request.obj;

            _logger.LogInformation(
       "Webhook values — Success: {Success}, Pending: {Pending}, MerchantOrderId: {OrderId}, AmountCents: {Amount}",
       obj.Success, obj.Pending, obj.Order?.MerchantOrderId, obj.AmountCents);

            var isSuccess = obj.Success && !obj.Pending;

            //  Idempotency: ignore already-processed transactions 
            var transactionId = obj.Id.ToString();

            var exists = await _unitOfWork.Payments
                .ExistsAsync(p => p.TransactionId == transactionId);

            if (exists)
            {
                _logger.LogInformation(
                    "Duplicate webhook ignored for transaction {TransactionId}", transactionId);
                return;
            }

            // Parse and validate MerchantOrderId 
            if (!int.TryParse(obj.Order.MerchantOrderId, out var orderId))
            {
                _logger.LogError(
                    "Invalid MerchantOrderId: {MerchantOrderId}", obj.Order.MerchantOrderId);
                throw new BadRequestException("Invalid OrderId");
            }

            //  Load order
            var order = await _unitOfWork.Orders.GetByIdWithItemsAsync(orderId);

            if (order == null)
            {
                _logger.LogError("Order not found: {OrderId}", orderId);
                throw new NotFoundException("Order");
            }

            // Skip already-completed orders 
            if (order.Status == OrderStatus.Completed)
            {
                _logger.LogInformation("Order already completed: {OrderId}", orderId);
                return;
            }

            //  Persist payment record
            var payment = new Payment
            {
                TransactionId = transactionId,
                OrderId = orderId,
                UserId = order.UserId,
                Amount = obj.AmountCents / 100m,
                Status = isSuccess ? "Success" : "Failed",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Payments.AddAsync(payment);

            // Update order status 
            //      
            order.Status = isSuccess ? OrderStatus.Completed : OrderStatus.Cancelled;

            // Enroll on success 
            if (isSuccess)
            {
                var courseItem = order.OrderItems.FirstOrDefault();

                if (courseItem == null)
                {
                    _logger.LogError("Order has no items: {OrderId}", orderId);
                    throw new BadRequestException("Order has no items");
                }

                var courseId = courseItem.CourseId;

                var alreadyEnrolled = await _unitOfWork.Enrollments
                    .ExistsAsync(e => e.CourseId == courseId && e.UserId == order.UserId);

                if (!alreadyEnrolled)
                {
                    var enrollment = new Enrollment
                    {
                        CourseId = courseId,
                        UserId = order.UserId,
                        EnrolledAt = DateTime.UtcNow,
                        ProgressPercentage = 0
                    };

                    await _unitOfWork.Enrollments.AddAsync(enrollment);
                }
                else
                {
                    _logger.LogInformation(
                        "User already enrolled in course {CourseId} — skipping.", courseId);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                "Webhook processed successfully for Order {OrderId} — Status: {Status}",
                orderId, order.Status);
        }
    }
}