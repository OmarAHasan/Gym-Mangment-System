using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetMemberShipsWithLodedData();
    }
}
