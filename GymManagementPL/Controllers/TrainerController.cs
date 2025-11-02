using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    //[Authorize]
    public class TrainerController : Controller
    {
        private readonly ITrainerServies _trainerServies;

        public TrainerController(ITrainerServies trainerServies) {
            this._trainerServies = trainerServies;
        }
        public ActionResult Index()
        {
            var trainers = _trainerServies.GetAll();
            return View(trainers);
        }

        public ActionResult TrainerDetails(int id) {

            // check if id is valid
            if (id <= 0) {

                TempData["ErrorMessage"] = "ID of Trainer Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerServies.GetTrainerDetails(id);
            if (trainer is null) {

                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        
        }

        public ActionResult Create() { 
        
            return View();
        
        }

        public ActionResult CreateTrainer(CreateTrainerViewModel createtrainer) {

            if (!ModelState.IsValid) {

                ModelState.AddModelError("DataInvalid", "Check Data And Missing Field");
                return View(nameof(Create), createtrainer);

            }

            var iscreated = _trainerServies.CreateTrainer(createtrainer);
            if (iscreated)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {

                TempData["ErrorMessage"] = "Trainer Failed To Create | Check Email & Phone";

            }

            return RedirectToAction(nameof(Index));

        }

        public ActionResult EditTrainer(int id) {

            // check if id is valid 
            if (id <= 0) {

                TempData["ErrorMessage"] = "ID of Trainer Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerServies.GetTrainerToUpdate(id);
            if (trainer is null) {

                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));      

            }

            return View(trainer);
        
        }

        [HttpPost]

        public ActionResult EditTrainer([FromRoute]int id, TrainerToUpdateViewModel traineredit) {

            // check Model state
            if (!ModelState.IsValid) {

                return View(traineredit);
            
            }


            bool isedited = _trainerServies.UpdateTrainer(id, traineredit);
            if (isedited)
            {

                TempData["SuccessMessage"] = "Trainer Edited Successfully";

            }
            else {

                TempData["ErrorMessage"] = "Member Failed To Edit";
            
            }

            return RedirectToAction(nameof(Index));
        
        }


        public ActionResult Delete(int id) {

            if (id <= 0) {

                TempData["ErrorMessage"] = "ID of Trainer Can't Be Zero Or Negative Number";
                return RedirectToAction(nameof(Index));

            }

            var trainer = _trainerServies.GetTrainerToUpdate(id);
            if (trainer is null)
            {

                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));

            }

            ViewBag.trainerid = id;
            return View();

        }

        [HttpPost]

        public ActionResult DeleteConfirmed([FromForm] int id) {

            var isdeleted = _trainerServies.RemoveTrainer(id);

            if (isdeleted)
            {

                TempData["SuccessMessage"] = "Trainer Deleted SuccessFully";
            }
            else {

                TempData["ErrorMessage"] = "Trainer Can Not Deleted";
            
            }

            return RedirectToAction(nameof(Index));


        }
    }
}
