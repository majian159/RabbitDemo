using System.Web.Mvc;

namespace Rabbit.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}