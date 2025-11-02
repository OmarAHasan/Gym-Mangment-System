using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetSessionsWithCategoryAndTrainer();

        // get sessionDetails with trainer and category

        Session? GetSessionDetailsWithTrainerAndCategory(int sessionid);

        // get count of Booking

        int GetCountOfBooking(int sessionid);
    }
}
