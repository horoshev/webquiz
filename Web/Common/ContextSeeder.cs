using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace Web.Common
{
    public static class ContextSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator));
            await roleManager.CreateAsync(new IdentityRole(Roles.Author));
            await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }

        public static async Task SeedAdminUser(UserManager<User> userManager)
        {
            const string? adminEmail = "admin@email.com";
            const string? adminPassword = "!admin";

            var admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var dbUser = userManager.Users.FirstOrDefault(u => u.Email == adminEmail);
            // await userManager.DeleteAsync(admin);
            if (dbUser is null)
            {
                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }

        public static async Task SeedCommonUser(UserManager<User> userManager)
        {
            const string? userEmail = "user@email.com";
            const string? userPassword = "!user";

            var user = new User
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true
            };

            var dbUser = userManager.Users.FirstOrDefault(u => u.Email == userEmail);
            // await userManager.DeleteAsync(dbUser);
            if (dbUser is null)
            {
                await userManager.CreateAsync(user, userPassword);
                await userManager.AddToRoleAsync(user, Roles.User);
            }
        }
    }
}