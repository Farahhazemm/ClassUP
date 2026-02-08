using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Instructor", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Failed to create role '{role}': " +
                            string.Join(", ", result.Errors.Select(e => e.Description))
                        );
                    }
                }
            }
        }
    }
}
