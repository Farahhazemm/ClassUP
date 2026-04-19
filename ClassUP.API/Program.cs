using ClassUP.API;
using ClassUP.Infrastructure;
using ClassUP.ApplicationCore;

var builder = WebApplication.CreateBuilder(args);

// External layers
builder.Services.AddApplicationConfig(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// API Layer
builder.Services.AddApiServices(builder.Configuration, builder.Host);

var app = builder.Build();

// Pipeline
app.UseApiPipeline();

// Seeder
await app.SeedAsync();

app.Run();