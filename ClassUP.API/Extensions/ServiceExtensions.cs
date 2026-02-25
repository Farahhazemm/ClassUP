using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;

namespace ClassUP.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 10;

                // User settings
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

              
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }

      
    }
}
