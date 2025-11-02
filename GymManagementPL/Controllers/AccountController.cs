using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AcountServiesViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServies _accountServies;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountServies accountServies, SignInManager<ApplicationUser> signInManager)
        {
            _accountServies = accountServies;
            _signInManager = signInManager;
        }


        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]

        public ActionResult Login(LoginViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            var user = _accountServies.ValidateUser(model);
            if (user is null)
            {

                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);

            }

            var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

            if (result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Not Allowed");

            if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Is Locked Out");

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {

            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));

        }

        public ActionResult AccessDenied()
        {

            return View();
        }
    }
}
