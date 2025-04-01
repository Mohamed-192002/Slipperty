using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin));
                //await roleManager.CreateAsync(new IdentityRole(Roles.Role_Employee));
                await roleManager.CreateAsync(new IdentityRole(Roles.Role_Customer));
            }
        }
    }
}
