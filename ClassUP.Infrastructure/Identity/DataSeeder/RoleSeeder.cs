using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Identity.DataSeeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(AppRoles.User))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.User));

            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
        }
    }
}
