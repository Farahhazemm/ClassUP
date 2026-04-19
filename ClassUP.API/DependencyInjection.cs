using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Helpers.Cloudniary;
using ClassUP.ApplicationCore.Helpers.JWT;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Infrastructure.Identity_Account.Email.Settings;
using ClassUP.Infrastructure.Payments;
using ClassUP.Infrastructure.Services.Videos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Refit;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace ClassUP.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostBuilder host)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            // Controllers + JSON
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(
                        new System.Text.Json.Serialization.JsonStringEnumConverter());
                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            // Identity
            services.ConfigureIdentity();

            // JWT
            services.Configure<JwtOptions>(
                configuration.GetSection("JWT"));

            var jwtOptions = configuration
                .GetSection("JWT")
                .Get<JwtOptions>();

            var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // External configs
            services.Configure<CloudinarySettings>(
                configuration.GetSection("CloudinarySettings"));

            services.Configure<MailSettings>(
                configuration.GetSection(nameof(MailSettings))
            );

            services.AddScoped<IVideoService, VideoService>();
            services.AddHttpContextAccessor();

            // Paymob
            services.Configure<PaymobSettings>(
                configuration.GetSection("Paymob"));

            services.AddRefitClient<IPaymobClient>()
                .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri("https://accept.paymob.com"));

            // Serilog
            host.UseSerilog((context, configurationBuilder) =>
                configurationBuilder.ReadFrom.Configuration(context.Configuration)
            );

            // Rate Limiting
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = 429;

                rateLimiterOptions.AddPolicy("iplimit", HttpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1)
                        })
                );

                rateLimiterOptions.AddPolicy("userlimit", HttpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: HttpContext.User.GetUserId(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1)
                        })
                );
            });

            return services;
        }
    }
}
