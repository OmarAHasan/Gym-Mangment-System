using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels
{
    public class BookingCreateVm
    {
        [Required] public int MemberId { get; set; }
        [Required] public int SessionId { get; set; }
    }
}
