using System.Web.Mvc;

namespace Rabbit.UserInterface.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Media()
        {
            return View();
        }
    }
}