using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer : GemUser
    {
        // HireDate - CreatedAt of GymUser

        public Specialties Specialties { get; set; }

        #region RelationShip 
        #region Session - Trainer

        public ICollection<Session> Sessions { get; set; } = null!;

        #endregion
        #endregion
    }
}
