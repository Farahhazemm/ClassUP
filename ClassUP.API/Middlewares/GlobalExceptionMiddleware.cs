using ClassUP.ApplicationCore.Exeptions;
using System.Text.Json;
namespace ClassUP.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is AppException appException)
            {
                context.Response.StatusCode = (int)appException.StatusCode;

                var response = new
                {
                    success = false,
                    error = appException.Message
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));

                return;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var defaultResponse = new
            {
                success = false,
                error = "An unexpected error occurred. Please try again later."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(defaultResponse));
        }
    }
}