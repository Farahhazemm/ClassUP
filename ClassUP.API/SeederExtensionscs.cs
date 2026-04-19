using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Identity.DataSeeder;
using Microsoft.AspNetCore.Identity;

namespace ClassUP.API
{
    public static class SeederExtensions
    {
        public static async Task SeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await RoleSeeder.SeedAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            await AdminSeeder.SeedAdminAsync(userManager, roleManager, configuration);
        }
    }
}
