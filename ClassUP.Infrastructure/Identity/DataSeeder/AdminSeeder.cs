using ClassUP.Domain.Constants;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Identity.DataSeeder
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
           
            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));

            
            var adminEmail = "admin@classup.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin != null) return;

            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Super",
                LastName = "Admin"
            };

            var password = "Admin123#@!"; 

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
