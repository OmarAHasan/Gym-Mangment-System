using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberShipViewModel
{
    public class CreateMemberShipViewModel
    {

        [Required(ErrorMessage = "Member Is Requierd")]
        public int MemberId { get; set; }
        [Required(ErrorMessage = "Plan Is Requierd")]
        public int PlanId { get; set; } 
    }
}
