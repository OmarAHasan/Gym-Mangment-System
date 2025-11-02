using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    //[Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionServies _sessionServies;

        public SessionController(ISessionServies sessionServies)
        {
            this._sessionServies = sessionServies;
        }
        public IActionResult Index()
        {
            var sessions = _sessionServies.GetAll();
            return View(sessions);
        }

        public ActionResult Details(int id)
        {
            // Check if the provided ID is valid
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(actionName: nameof(Index));
            }

            // Attempt to retrieve the session from a service
            var session = _sessionServies.GetSessionById(id);

            // Check if the session was found
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(actionName: nameof(Index));
            }

            // If everything is OK, return the View with the session model
            return View(session);
        }


        public ActionResult Create() {
            LoadDropdownForCategories();
            LoadDropdownForTrainers();
            return View();

        }

        [HttpPost]

        public ActionResult Create(CreateSessionViewModel createseeesion) {


            if (!ModelState.IsValid) {

                LoadDropdownForCategories();
                LoadDropdownForTrainers();
                return View(createseeesion);
            
            }

            bool isCreated = _sessionServies.CeateSession(createseeesion);
            if (isCreated) { 
            
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Session";
                LoadDropdownForCategories();
                LoadDropdownForTrainers();
                return View(createseeesion);

            }

                   
        }


        public ActionResult Edit(int id) {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(actionName: nameof(Index));
            }

            var session = _sessionServies.GetSessionToUpdate(id);
            if (session is null) { 
            
                TempData["ErrorMessage"] = "Session Not Found Or You Can't Update It";
                return RedirectToAction(actionName: nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public ActionResult Edit([FromRoute] int id, SessionUpdateViewModel UpdatedSession)
        {

            if (!ModelState.IsValid)
            {
                LoadDropdownForTrainers();
                return View(model: UpdatedSession);
            }


            var result = _sessionServies.UpdateSession(id, UpdatedSession);

            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To updated";
            }


            return RedirectToAction(actionName: nameof(Index));
        }

        public ActionResult Delete(int id)
        {
        
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(actionName: nameof(Index));
            }

            
            var session = _sessionServies.GetSessionById( id);

            
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(actionName: nameof(Index));
            }

            // Pass the session ID to the view for the confirmation form
            ViewBag.SessionId = session.Id;
            return View();
        }


        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            
            var result = _sessionServies.RemoveSession(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Can not Be Deleted";
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        #region HelperMethods

        private void LoadDropdownForTrainers() {


            var trainers = _sessionServies.GetAllTrainersForSelect();
            ViewBag.trainers = new SelectList(trainers, "Id", "Name");


        }

        private void LoadDropdownForCategories()
        {


            var categories = _sessionServies.GetAllCategoriesForSelect();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
        }
        #endregion
    }
}
