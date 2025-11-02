using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    //[Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanServies _planServies;

        public PlanController(IPlanServies planServies)
        {
            this._planServies = planServies;
        }
        public IActionResult Index()
        {
            var plans = _planServies.GetAll();
            return View(plans);
        }

        public ActionResult Details(int id)
        {

            if (id <= 0)
            {

                TempData["ErrorMessage"] = "ID of Member Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planServies.GetById(id);

            if (plan is null)
            {

                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));


            }

            return View(plan);

        }

        public ActionResult Edit(int id)
        {

            if (id <= 0)
            {

                TempData["ErrorMessage"] = "ID of Member Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planServies.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));


            }

            return View(plan);
        }


        [HttpPost]
        public ActionResult Edit(int id, PlanToUpdateViewModel updateplan) {

            if (!ModelState.IsValid) { 
            
                ModelState.AddModelError("WrongData", "Check Data And Missing Field");
                return View(updateplan);
            }

            var isupdated = _planServies.UpdatePlan(id, updateplan);
            if (isupdated)
            {

                TempData["SuccessMessage"] = "Plan Updated Successfully";

            }
            else { 
            
                TempData["ErrorMessage"] = "Failed To Update Plan";
            }

            return RedirectToAction(nameof(Index));

        }

        public ActionResult IsActivie(int id) { 
        
            var plan = _planServies.ToggelStatus(id);
            if (plan)
            {
                TempData["SuccessMessage"] = "Plan Status Changed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Change Plan Status";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
