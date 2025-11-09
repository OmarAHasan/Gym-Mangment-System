using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GemUser
    {
        // joinDate == CreatedAt

        public string Photo { get; set; } = null!;

        #region HealthRecord RelationShip
        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion
        #region Member - MemberShip

        public ICollection<MemberShip> MemberShips { get; set; } = null!;

        #endregion

        #region Member - MemberSession 

        public ICollection<MemberShip> MemberSessions = null!;

        #endregion

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
