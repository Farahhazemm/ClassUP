using ClassUP.API.Auth;
using ClassUP.API.Extensions;
using ClassUP.ApplicationCore;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Infrastructure;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.ExternalServices;
using ClassUP.Infrastructure.Identity;
using ClassUP.Infrastructure.Repository;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.ConfigureIdentity();

//---------------------------------------
string Key = builder.Configuration["JWT :SecritKey"];
var KeyinBytes = Encoding.UTF8.GetBytes(Key);
var signinKey = new SymmetricSecurityKey(KeyinBytes);
// 1.1 How Validation 
builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  // return Un authrized 
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op => // 1.2 How Verfied Key
{
    op.SaveToken = true;
    op.RequireHttpsMetadata = false;
    op.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT : Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT :Audience"],
        IssuerSigningKey = signinKey,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,


    };

   
    
});
//---------------------------------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddScoped<IVideoService, VideoService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

// Seed Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await ClassUP.Infrastructure.Identity.RoleSeeder.SeedRolesAsync(services);
}

app.Run();
