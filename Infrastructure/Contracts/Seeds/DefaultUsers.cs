using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts.Seeds
{
    public class DefaultUsers
    {
        public static async Task SeedAdminsUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUsers = new List<ApplicationUser>();
            defaultUsers.Add(new ApplicationUser
            {
                UserName = "Ahmed",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FirstName = "Ahmed",
                LastName = "Mohamed",
                PhoneNumber = "201234567889",
                Address = "Cairo",
            });

            foreach (var user in defaultUsers)
            {
                var userExists = await userManager.FindByEmailAsync(user.Email);

                if (userExists is null)
                {
                    try
                    {
                        await userManager.CreateAsync(user, "111111");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    //await userManager.AddToRolesAsync(user, new List<string> { Roles.Role_Admin, Roles.Role_Employee, Roles.Role_Customer });
                    await userManager.AddToRolesAsync(user, new List<string> { Roles.Role_Admin });
                }
            }
        }
    }
}
