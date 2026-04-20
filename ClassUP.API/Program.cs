using ClassUP.API;
using ClassUP.ApplicationCore;
using ClassUP.Infrastructure;
using ClassUP.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// External layers
builder.Services.AddApplicationConfig(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration, builder.Host);

var app = builder.Build();


//  CREATE DATABASES FIRST

using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connString = config["ConnectionStrings:MasterConnection"];

    using var connection = new SqlConnection(connString);
    connection.Open();

    using var cmd = connection.CreateCommand();

    cmd.CommandText = @"
        IF DB_ID('ClassUP') IS NULL CREATE DATABASE ClassUP;
        IF DB_ID('ClassUPJob') IS NULL CREATE DATABASE ClassUPJob;
    ";

    cmd.ExecuteNonQuery();
}


// MIGRATION

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retry = 10;

    while (retry > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch
        {
            retry--;
            Thread.Sleep(5000);
        }
    }
}



//  SEED

await app.SeedAsync();



// PIPELINE 

app.UseApiPipeline();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();