using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Classes
{

    // specific repo
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public int GetCountOfBooking(int sessionid)
        {
            return _dbContext.MemberSessions.Count(X => X.SessionId == sessionid);
        }

        public Session? GetSessionDetailsWithTrainerAndCategory(int sessionid)
        {
            return _dbContext.Sessions.Include(X => X.Category)
                                              .Include(X => X.SessionTrainer).FirstOrDefault(X => X.Id == sessionid);
        }

        public IEnumerable<Session> GetSessionsWithCategoryAndTrainer()
        {
            return _dbContext.Sessions.Include(X => X.Category)
                                              .Include(X => X.SessionTrainer).ToList();
        }
    }
}
