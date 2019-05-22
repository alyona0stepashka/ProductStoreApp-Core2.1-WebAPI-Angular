using System;
using Microsoft.AspNetCore.Identity; 
using System.Threading.Tasks;
using App.Models;

namespace App.DAL.Initializer
{
    public class ApplicationDbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            const string adminEmail = "admin@mail.ru";
            const string password = "Parol_01";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true,
                    DateOfRegisters = DateTime.Now
                };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
