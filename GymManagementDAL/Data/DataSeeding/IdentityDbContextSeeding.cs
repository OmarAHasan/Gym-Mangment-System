using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeeding
{
    public static class IdentityDbContextSeeding
    {

        public static async Task<bool> SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (userManager.Users.Any() && roleManager.Roles.Any())
                    return false;

                if (!roleManager.Roles.Any())
                {
                    var roles = new[] { "SuperAdmin", "Admin" };

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                }

                if (!userManager.Users.Any())
                {
                    var mainAdmin = new ApplicationUser
                    {
                        FirstName = "Omar",
                        LastName = "Ahmed",
                        UserName = "OmarAhmed",
                        Email = "Omar@gmail.com",
                        PhoneNumber = "01112223456",
                    };

                    await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");

                    var admin = new ApplicationUser
                    {
                        FirstName = "Yasser",
                        LastName = "Moahmed",
                        UserName = "YasserMohamed",
                        Email = "Yaseer@gmail.com",
                        PhoneNumber = "01010123120",
                    };

                    await userManager.CreateAsync(admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seed Failed: {ex}");
                return false;
            }
        }

    }
}
