using ClassUP.ApplicationCore.IRepository;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using ClassUP.ApplicationCore;
using ClassUP.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConc")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
