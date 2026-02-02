using ClassUP.API.Auth;
using ClassUP.ApplicationCore;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Infrastructure;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConc")));


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
