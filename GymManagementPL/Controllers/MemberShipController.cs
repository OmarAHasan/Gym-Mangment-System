using GymManagementBLL.Services.classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.WebSockets;

namespace GymManagementPL.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipServies memberShipServies;

        public MemberShipController(IMemberShipServies memberShipServies)
        {
            this.memberShipServies = memberShipServies;
        }
        public ActionResult Index()
        {
            var memberShips = memberShipServies.GetAll();
            return View(memberShips);
        }

        public ActionResult Create() {
            LoadMemberList();
            LoadPlanList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateMemberShipViewModel createmembership) {
            bool isCreated = memberShipServies.CreateMemberShip(createmembership);
            if (isCreated) {
                TempData["SuccessMessage"] = "Membership Created Successfully";
                return  RedirectToAction(nameof(Index));    
            }
            else {
                TempData["ErrorMessage"] = "Membership Creation Failed";
                 LoadMemberList();
                 LoadPlanList();
                return View(createmembership);
            }


        }


        public ActionResult Remove(int memberid,int planid) { 
        
            bool isRemoved = memberShipServies.RemoveMemberShip( memberid, planid);
            if (isRemoved)
            {
                TempData["SuccessMessage"] = "Membership Removed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Membership Removal Failed";
            }
            return RedirectToAction(nameof(Index));


        }

        #region HelperMethods

        private void LoadMemberList() {
            var members = memberShipServies.GetMembersFroSelect();
            ViewBag.members = new SelectList(members, "Id", "Name");

        }

        private void LoadPlanList() {

            var plans = memberShipServies.GetPlansForSelect();
            ViewBag.plans = new SelectList(plans, "Id", "Name");

        }

        #endregion


    }
}
