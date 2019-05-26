using System;
using Microsoft.AspNetCore.Identity; 
using System.Threading.Tasks;
using App.Models;
using App.DAL.Data;
using System.Linq;
using App.DAL.Interfaces;

namespace App.DAL.Initializer
{
    public class ApplicationDbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IUnitOfWork db)
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
            var file_id = 0;
            if (!(await db.FileModels.FindAsync(m => m.Name == "no-image.jpg")).Any())  //????
            {
                file_id = (await db.FileModels.CreateAsync(new FileModel
                {
                    Name = "no-image.jpg",
                    Path = "/Images/App/no-image.jpg"
                })).Id;
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true,
                    DateOfRegisters = DateTime.Now,
                    LastName = "admin",
                    FirstName = "admin",
                    FileModelId = file_id
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
