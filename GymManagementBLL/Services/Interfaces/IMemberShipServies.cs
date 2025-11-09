using GymManagementBLL.Helpers;
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
        IEnumerable<MemberShip> GetAll();
        bool CreateMemberShip(CreateMemberShipViewModel createMemberShipViewModel);
        bool RemoveMemberShip(int memberid , int planid);

        IEnumerable<PlanSelectViewModel> GetPlansForSelect();
        IEnumerable<MemberSelectViewModel> GetMembersFroSelect();
        Task<IEnumerable<MemberShip>> GetAllAsync();
        Task<MemberShip> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(int memberId, int planId, DateTime startDate);
        Task<OperationResult> CancelAsync(int membershipId);
        Task<bool> MemberHasActiveMembershipAsync(int memberId);
    }
}
