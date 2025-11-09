using GymManagementBLL.Services.classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Data.Repositories.Classes;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
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
        private readonly IMemberShipServies _membershipService;
        private readonly IPlanRepo _planRepo; 
        private readonly IMemberRepository _memberRepo; 

        public PlanController(IMemberShipServies membershipService)
        {
            _membershipService = membershipService;

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Members = await _membershipService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MembershipCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Members = await _membershipService.GetAllAsync();
                ViewBag.Plans = await _planRepo.GetAllActiveAsync();
                return View(vm);
            }

            var result = await _membershipService.CreateAsync(vm.MemberId, vm.PlanId, vm.StartDate);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                ViewBag.Members = await _membershipService.GetAllAsync();
                ViewBag.Plans = await _planRepo.GetAllActiveAsync();
                return View(vm);
            }
            TempData["Success"] = "Membership created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _membershipService.CancelAsync(id);
            if (!result.Success) TempData["Error"] = result.ErrorMessage;
            else TempData["Success"] = "Membership cancelled.";
            return RedirectToAction(nameof(Index));
        }
    }
}
