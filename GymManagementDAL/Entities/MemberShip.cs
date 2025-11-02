using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberShip : BaseEntity
    {

        // startDate == CreatedAt

        public DateTime EndDate { get; set; }

        // read only attribute
        public string Status { get {

                if (EndDate <= DateTime.Now)
                    return "Expired";
                else
                    return "Active";
            
               } }
        public int MemebreId { get; set; }
        public Member Memebre { get; set; } = null!;
        
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

    }
}
