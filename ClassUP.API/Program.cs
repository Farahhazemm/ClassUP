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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddScoped<IVideoService, VideoService>();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

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
