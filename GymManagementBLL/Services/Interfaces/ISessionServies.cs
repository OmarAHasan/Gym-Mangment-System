using GymManagementBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionServies
    {
        IEnumerable<SessionViewModel> GetAll();

        SessionViewModel? GetSessionById(int sessionid);

        bool CeateSession(CreateSessionViewModel createSession);

        SessionUpdateViewModel? GetSessionToUpdate(int sessionid);

        bool UpdateSession(int sessionid, SessionUpdateViewModel sessionupdate);

        bool RemoveSession(int sessionid);

        IEnumerable<TrainerSelectViewModel> GetAllTrainersForSelect();
        IEnumerable<CategorySelectViewModel> GetAllCategoriesForSelect();
    }
}
