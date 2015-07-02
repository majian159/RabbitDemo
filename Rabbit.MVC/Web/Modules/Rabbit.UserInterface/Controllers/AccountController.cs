using Rabbit.Components.Security;
using Rabbit.Infrastructures.Security;
using Rabbit.UserInterface.Models;
using Rabbit.UserInterface.Services;
using Rabbit.UserInterface.ViewModels;
using System.Web.Mvc;

namespace Rabbit.UserInterface.Controllers
{
    public class AccountController : Controller
    {
        #region Field

        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        #endregion Field

        #region Constructor

        public AccountController(IAuthenticationService authenticationService, IAccountService accountService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _accountService = accountService;
            _userService = userService;
        }

        #endregion Constructor

        #region Action

        public ActionResult SignIn(string returnUrl)
        {
            return View(new SignInViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //验证失败。
            if (!_accountService.Exist(model.Account, model.Password))
            {
                ModelState.AddModelError(string.Empty, "用户名或密码错误。");
                return View(model);
            }

            _authenticationService.SignIn(new UserModel { Identity = model.Account, UserName = model.Account }, model.Remember);

            if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                return RedirectToAction("Index", "Admin");
            return Redirect(model.ReturnUrl);
        }

        public ActionResult SignOut(string returnUrl)
        {
            _authenticationService.SignOut();
            return RedirectToAction("SignIn", new { returnUrl });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_accountService.Exist(model.Account))
            {
                ModelState.AddModelError(string.Empty, "账号已经存在，请换一个试试。");
                return View(model);
            }
            var user = UserRecord.Create(model.UserName, AccountRecord.Create(model.Account, model.Password));
            _userService.Create(user);

            return RedirectToAction("SignIn");
        }

        #endregion Action
    }
}