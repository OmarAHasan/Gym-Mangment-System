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

        public static bool SeedData(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            try
            {

                // check if tables has data or not 

                var hasuser = userManager.Users.Any();
                var hasrole = roleManager.Roles.Any();
                if (hasuser && hasrole) return false;

                if (!hasrole)
                {

                    var roles = new List<IdentityRole> {

                        new() { Name = "SuperAdmin"},
                        new() { Name = "Admin"},

                };

                    foreach (var role in roles)
                    {

                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {

                            roleManager.CreateAsync(role).Wait();

                        }
                    }

                }


                if (!hasuser)
                {


                    var mainAdmin = new ApplicationUser
                    {

                        FirstName = "Omar",
                        LastName = "Ahmed",
                        UserName = "OmarAhmed",
                        Email = "Omar@gmail.com",
                        PhoneNumber = "01112223456",
                    };
                    userManager.CreateAsync(mainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();


                    var Admin = new ApplicationUser
                    {

                        FirstName = "Yasser",
                        LastName = "Moahmed",
                        UserName = "YasserMohamed",
                        Email = "Yaseer@gmail.com",
                        PhoneNumber = "01010123120",
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seed Failed {ex}");
                return false;
            }
        }
    }
}
