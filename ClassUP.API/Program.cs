using ClassUP.API.Auth;
using ClassUP.ApplicationCore;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Infrastructure;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.ExternalServices;
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

// For Video Upload / Delete
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddScoped<IVideoService, VideoService>();
/*
 Add Scheme => away to Authenticate Takes Two parameters 
1 : Name in shape string "Name is here "
2 : Configure Option 

also need Two genaric Parameters <,>
1: Toptions => default op (AuthenticationSchemeOptions)
2: THandeler => responsapol for Validation 
*/
/*builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandelercs>("Basic", null);*/
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
