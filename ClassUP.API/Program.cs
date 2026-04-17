
using ClassUP.API.Extensions;
using ClassUP.API.Middlewares;
using ClassUP.ApplicationCore;
using ClassUP.ApplicationCore.Helpers.Cloudniary;
using ClassUP.ApplicationCore.Helpers.JWT;
using ClassUP.ApplicationCore.IServices.Payments;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Identity.DataSeeder;
using ClassUP.Infrastructure.Identity_Account.Email.Settings;
using ClassUP.Infrastructure.Payments;
using ClassUP.Infrastructure.Services.Videos;
using CloudinaryDotNet;
using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Refit;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


// Add Services

builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddApplication();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.ConfigureIdentity();


// JWT Authentication
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JWT"));

var jwtOptions = builder.Configuration
    .GetSection("JWT")
    .Get<JwtOptions>();

var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
var signingKey = new SymmetricSecurityKey(keyBytes);

builder.Services.AddAuthentication(options =>
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// External Services Config

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection(nameof(MailSettings))
);

builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddHttpContextAccessor();

// DI For Payment

builder.Services.Configure<PaymobSettings>(
    builder.Configuration.GetSection("Paymob"));

builder.Services.AddRefitClient<IPaymobClient>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri("https://accept.paymob.com"));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);
// Rate Limiting
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = 429; // Too Many Requests

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


    //rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
    //{
    //    options.PermitLimit = 10; // Maximum number of concurrent requests
    //    options.QueueLimit = 5; // No queuing, reject immediately when limit is reached 
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Process queued requests in the order they were received
    //});

    //rateLimiterOptions.AddTokenBucketLimiter("tokenBucket", options =>
    //{
    //    options.TokenLimit = 100; // Maximum number of tokens in the bucket
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Process queued requests in the order they were received
    //    options.QueueLimit = 0; // No queuing, reject immediately when limit is reached 
    //    options.ReplenishmentPeriod = TimeSpan.FromMinutes(1); // Replenish tokens every minute
    //    options.TokensPerPeriod = 100; // Number of tokens to add each replenishment period
    //    options.AutoReplenishment = true; // Automatically replenish tokens 
    //});


    //rateLimiterOptions.AddSlidingWindowLimiter("slidingWindow", options =>
    //{
    //    options.Window = TimeSpan.FromSeconds(30); // Time window for rate limiting
    //    options.SegmentsPerWindow = 6; // Number of segments within the window
    //    options.PermitLimit = 20; // Maximum number of requests allowed within the time window
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Process queued requests in the order they were received
    //    options.QueueLimit = 0; // No queuing, reject immediately when limit is reached
    //});

});
var app = builder.Build();


// Middleware
app.UseCors("AllowAll");
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseRateLimiter();

// Hangfire Dashboard with Basic Authentication

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
       [
        new HangfireCustomBasicAuthenticationFilter
        {
        User = app.Configuration.GetValue<string>("HangfireSettings:username"),
        Pass = app.Configuration.GetValue<string>("HangfireSettings:password")

    }
        ],
    DashboardTitle = "ClassUP Background Jobs",
    IsReadOnlyFunc = (DashboardContext context) => true // Make the dashboard read-only

});
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// Seeder


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedAsync(roleManager);

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var configuration = services.GetRequiredService<IConfiguration>();
    await AdminSeeder.SeedAdminAsync(userManager, roleManager, configuration);
}

app.Run();