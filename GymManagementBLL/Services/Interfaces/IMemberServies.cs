using GymManagementBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberServies
    {
        IEnumerable<MemberViewModel> GetAll();

        bool CreateMember(CreateMemberViewModel createMemberViewModel);
        MemberViewModel? GetMemberDetails(int id);

        HealthRecordViewModel? GetMemberHealthRecord(int memberid);

        MemberToUpdateViewModel? MemberToUpdate(int id);

        bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberDetails);

        bool RemoveMember(int memberid);
    }
}
