using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AcountServiesViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{
    public class AccountServies : IAccountServies
    {
        public AccountServies(UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
        }

        public UserManager<ApplicationUser> _UserManager { get; }

        public ApplicationUser? ValidateUser(LoginViewModel login)
        {
            var user = _UserManager.FindByEmailAsync(login.Email).Result;

            if (user is null) return null;
            var ispasswordvalid = _UserManager.CheckPasswordAsync(user, login.Password).Result;

            return ispasswordvalid ? user : null;
        }
    }
}
