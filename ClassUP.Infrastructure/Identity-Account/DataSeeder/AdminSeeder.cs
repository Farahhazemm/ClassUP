using ClassUP.Domain.Constants;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Identity.DataSeeder
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            // Ensure admin role exists
            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));

            // Read cred from config =>Env Vars
            var adminEmail = configuration["Admin:Email"];
            var password = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Admin credentials are missing from configuration.");

            // no duplication
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin != null) return;

            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create Admin user: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign Admin role: {errors}");
            }
        }
    }
}
