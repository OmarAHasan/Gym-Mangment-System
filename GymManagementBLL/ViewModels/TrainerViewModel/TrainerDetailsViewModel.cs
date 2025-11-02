using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModel
{
    public class TrainerDetailsViewModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
