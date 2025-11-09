using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels
{
    public class MembershipCreateVm
    {
        [Required] public int MemberId { get; set; }
        [Required] public int PlanId { get; set; }
        [Required][DataType(DataType.Date)] public DateTime StartDate { get; set; }
    }
}
