using GymManagementBLL.ViewModels.AcountServiesViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IAccountServies
    {

        ApplicationUser? ValidateUser(LoginViewModel login);
    }
}
