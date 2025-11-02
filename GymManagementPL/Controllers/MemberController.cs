using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberServies _memberServies;

        // ask clr to injicte object from member servies

        public MemberController(IMemberServies memberServies)
        {
            this._memberServies = memberServies;
        }
        public ActionResult Index()
        {
            var members = _memberServies.GetAll();
            return View(members);
        }

        public ActionResult MemberDetails(int id) {

            // if id <= 0 
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID of Member Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberServies.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View (member);
        }

        public ActionResult HealthRecordDetails(int id) {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID of Member Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var healthrecord = _memberServies.GetMemberHealthRecord(id);
            if (healthrecord is null)
            {
                TempData["ErroMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(healthrecord);
        }

        public ActionResult Create() { 
        
            return View();
        }

        public ActionResult CreateMember(CreateMemberViewModel createmember) {

            // check state is valid 
            if (!ModelState.IsValid) {

                ModelState.AddModelError("DataInvalid", "Check Data And Missing Field");
                return View(nameof(Create), createmember);
            
            }

            bool iscreated = _memberServies.CreateMember(createmember);
            if (iscreated)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else {

                TempData["ErrorMessage"] = "Member Failed To Create | Check Email & Phone";
            
            }

            return RedirectToAction(nameof(Index));
        
        }


        public ActionResult MemberEdit(int id) {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID of Member Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberServies.MemberToUpdate(id);
            if (member is null) {

                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);

        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id, MemberToUpdateViewModel memberupdate) {

            if (!ModelState.IsValid) {

                return View(memberupdate);
            }
          
            bool updatedmember = _memberServies.UpdateMemberDetails(id, memberupdate);
            if (updatedmember)
            {

                TempData["SuccessMessage"] = "Member Created Successfully";
                
            }
            else {
                TempData["ErrorMessage"] = "Member Failed To create";
            }

            return RedirectToAction(nameof(Index));
        }


        public ActionResult Delete(int id) {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID of Member Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberServies.GetMemberDetails(id);
            if (member is null) {

                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.memberid = member.Id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id) {

            var deleted = _memberServies.RemoveMember(id);

            if (deleted)
            {

                TempData["SuccesMessage"] = "Member Deletd Successfully";

            }
            else {

                TempData["ErrorMessage"] = "Member Can Not Deleted";
            
            }
        
            return RedirectToAction(nameof(Index));
        }
    }
}
