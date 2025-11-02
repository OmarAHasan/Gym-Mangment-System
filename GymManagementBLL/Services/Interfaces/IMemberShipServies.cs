using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberShipServies
    {
        IEnumerable<MemberShipViewModel> GetAll();
        bool CreateMemberShip(CreateMemberShipViewModel createMemberShipViewModel);
        bool RemoveMemberShip(int memberid , int planid);

        IEnumerable<PlanSelectViewModel> GetPlansForSelect();
        IEnumerable<MemberSelectViewModel> GetMembersFroSelect();
    }
}
