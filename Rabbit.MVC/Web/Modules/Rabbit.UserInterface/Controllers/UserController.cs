using Rabbit.Web.Mvc.UI.Admin;
using System.Web.Mvc;

namespace Rabbit.UserInterface.Controllers
{
    [Authorize]
    [Admin]
    public class UserController : Controller
    {
        public ActionResult UserList()
        {
            return View();
        }

        public ActionResult UserProfile()
        {
            return View();
        }
    }
}