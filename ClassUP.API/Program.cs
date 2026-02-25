
using ClassUP.API.Extensions;
using ClassUP.API.Middlewares;
using ClassUP.ApplicationCore;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.ExternalServices;
using ClassUP.Infrastructure.Identity.DataSeeder;
using ClassUP.Infrastructure.Identity_Account.Email.Settings;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add Services

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.ConfigureIdentity(); // Identity + EF DbContext


// JWT Authentication

var jwtKey = builder.Configuration["JWT:SigningKey"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
var signingKey = new SymmetricSecurityKey(keyBytes);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
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

var app = builder.Build();


// Middleware

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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