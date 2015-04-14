using Rabbit.Components.Security;
using Rabbit.UserInterface.ViewModels;
using System.Web.Mvc;

namespace Rabbit.UserInterface.Controllers
{
    public class AccountController : Controller
    {
        #region Field

        private readonly IAuthenticationService _authenticationService;

        #endregion Field

        #region Constructor

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #endregion Constructor

        #region Action

        public ActionResult SignIn()
        {
            return View();
        }

        [ActionName("SignIn")]
        [HttpPost]
        public ActionResult SignInPost(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _authenticationService.SignIn(new UserModel { Identity = model.UserName, UserName = model.UserName }, model.Remember);
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult SignOut()
        {
            _authenticationService.SignOut();
            return RedirectToAction("SignIn");
        }

        #endregion Action
    }
}