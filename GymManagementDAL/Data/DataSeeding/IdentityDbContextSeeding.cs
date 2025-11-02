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

                        FirstName = "Mohamed",
                        LastName = "Hisham",
                        UserName = "MohamedHisham",
                        Email = "Mohamed@gmail.com",
                        PhoneNumber = "01021840100",
                    };
                    userManager.CreateAsync(mainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();


                    var Admin = new ApplicationUser
                    {

                        FirstName = "Ahmed",
                        LastName = "Ali",
                        UserName = "AhmedAli",
                        Email = "Ahmed@gmail.com",
                        PhoneNumber = "01021840569",
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
