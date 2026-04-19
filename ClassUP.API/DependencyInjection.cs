using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.Helpers.Cloudniary;
using ClassUP.ApplicationCore.Helpers.JWT;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Infrastructure.Identity_Account.Email.Settings;
using ClassUP.Infrastructure.Payments;
using ClassUP.Infrastructure.Services.Videos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Refit;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
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
            // CORS (DEV ONLY)
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
            services.Configure<JwtOptions>(configuration.GetSection("JWT"));

            var jwtOptions = configuration.GetSection("JWT").Get<JwtOptions>();

            var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true; // ⚠️ production recommended

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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ClassUP API",
                    Version = "v1",
                    Description = "E-learning API for courses, categories, lectures, users",
                    Contact = new OpenApiContact
                    {
                        Name = "Farah Hazem",
                        Email = "farahhazem58@gmail.com"
                    }
                });

                options.UseInlineDefinitionsForEnums();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer {token}"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "ClassUP.API.xml");
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);

                var appXml = Path.Combine(AppContext.BaseDirectory, "ClassUP.ApplicationCore.xml");
                if (File.Exists(appXml))
                    options.IncludeXmlComments(appXml);
            });

            // Configurations
            services.Configure<CloudinarySettings>(
                configuration.GetSection("CloudinarySettings"));

            services.Configure<MailSettings>(
                configuration.GetSection(nameof(MailSettings)));

            services.Configure<PaymobSettings>(
                configuration.GetSection("Paymob"));

            // Services
            services.AddScoped<IVideoService, VideoService>();
            services.AddHttpContextAccessor();

            // Refit
            services.AddRefitClient<IPaymobClient>()
                .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri("https://accept.paymob.com"));

            // Serilog
            host.UseSerilog((context, loggerConfig) =>
                loggerConfig.ReadFrom.Configuration(context.Configuration));

            // Rate Limiting
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = 429;

                options.AddPolicy("iplimit", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy("userlimit", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.User?.Identity?.Name ?? "anonymous",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            return services;
        }
    }
}