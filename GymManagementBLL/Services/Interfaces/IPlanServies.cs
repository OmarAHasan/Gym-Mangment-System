using GymManagementBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanServies
    {
        // getAll
       IEnumerable<PlanViewModel> GetAll();

        // getById 
        PlanViewModel? GetById(int id);

        // getPlanDetails
        PlanToUpdateViewModel? GetPlanToUpdate(int planid);

        //UpdatePlan
        bool UpdatePlan(int planid, PlanToUpdateViewModel UpdatePlan);

        //ActivatePlan

        bool ToggelStatus(int planid);
    }
}
