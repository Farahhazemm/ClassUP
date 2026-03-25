using ClassUP.ApplicationCore.DTOs.Requests.Payments;
using ClassUP.ApplicationCore.DTOs.Responses.Payments;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.IServices.Payments;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClassUP.Infrastructure.Payments
{
    public class PaymobService : IPaymentService
    {
        private readonly IPaymobClient _client;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaymobSettings _settings;
        private readonly PaymobHmacService _hmac;
        private readonly ILogger<PaymobService> _logger;

        public PaymobService(
            IPaymobClient client,
            IUnitOfWork unitOfWork,
            IOptions<PaymobSettings> settings,
            PaymobHmacService hmac,
            ILogger<PaymobService> logger)
        {
            _client = client;
            _unitOfWork = unitOfWork;
            _settings = settings.Value;
            _hmac = hmac;
            _logger = logger;
        }

        public async Task<PaymentResponseDTO> CreatePaymentAsync(int courseId, string userId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException("Course");

            // Free course
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

                return new PaymentResponseDTO
                {
                    IsFreeCourse = true
                };
            }

            //  Create Order in DB
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

            // Paymob Auth
            var auth = await _client.GetAuthToken(new
            {
                api_key = _settings.ApiKey
            });

            // Create Paymob Order
            var paymobOrder = await _client.CreateOrder(new
            {
                auth_token = auth.Token,
                delivery_needed = "false",
                amount_cents = (int)(course.Price * 100),
                merchant_order_id = orderEntity.Id.ToString()
            });

            // Create Payment Key
            var paymentKey = await _client.CreatePaymentKey(new
            {
                auth_token = auth.Token,
                amount_cents = (int)(course.Price * 100),
                order_id = paymobOrder.Id,
                currency = "EGP",
                integration_id = _settings.IntegrationId,
                billing_data = new
                {
                    email = "user@email.com",
                    first_name = "User",
                    last_name = "Name",
                    phone_number = "01000000000",
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

            return new PaymentResponseDTO
            {
                OrderId = orderEntity.Id,
                PaymentUrl =
                    $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentKey.Token}"
            };
        }

        public async Task HandleWebhookAsync(PaymobWebhookRequestDTO request)
        {
            // Validate HMAC
            if (!_hmac.IsValid(request))
            {
                _logger.LogWarning("Invalid HMAC detected");
                throw new InvalidHmacException();
            }

            var transactionId = request.Obj.Id;

            // Idempotency
            var exists = await _unitOfWork.Payments.ExistsAsync(p => p.TransactionId == transactionId);
            if (exists) return;

            // Get OrderId safely
            var orderId = int.Parse(request.Obj.Order.MerchantOrderId);

            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order");

            //  Create Payment
            var payment = new Payment
            {
                TransactionId = transactionId,
                OrderId = orderId,
                UserId = order.UserId,
                Amount = request.Obj.AmountCents / 100m,
                Status = request.Obj.Success ? "Success" : "Failed",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Payments.AddAsync(payment);

            //  Update Order Status
            order.Status = request.Obj.Success
                ? OrderStatus.Completed
                : OrderStatus.Cancelled;

            //  Enrollment on success
            if (request.Obj.Success)
            {
                var courseItem = order.OrderItems.FirstOrDefault();

                if (courseItem == null)
                    throw new BadRequestException("Order has no items");

                var courseId = courseItem.CourseId;
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    UserId = order.UserId,
                    EnrolledAt = DateTime.UtcNow,
                    ProgressPercentage = 0
                };

                await _unitOfWork.Enrollments.AddAsync(enrollment);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}