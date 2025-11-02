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
    public class MemberShipRepository : GenericRepository<MemberShip> , IMemberShipRepository
    {
        private readonly GymDbContext dbContext;

        public MemberShipRepository(GymDbContext dbContext) : base(dbContext)
        {
            
            this.dbContext = dbContext;
        }

        public IEnumerable<MemberShip> GetMemberShipsWithLodedData()
        {
            return dbContext.memberShips
                .Include(ms => ms.Memebre)
                .Include(ms => ms.Plan)
                .ToList();
        }

    }
}
