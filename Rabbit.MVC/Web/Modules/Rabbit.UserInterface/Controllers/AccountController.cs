using Rabbit.Components.Security;
using Rabbit.Infrastructures.Security;
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

        public ActionResult SignIn(string returnUrl)
        {
            return View(new SignInViewModel { ReturnUrl = returnUrl });
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
            if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                return RedirectToAction("Index", "Admin");
            return Redirect(model.ReturnUrl);
        }

        public ActionResult SignOut(string returnUrl)
        {
            _authenticationService.SignOut();
            return RedirectToAction("SignIn", new { returnUrl });
        }

        #endregion Action
    }
}