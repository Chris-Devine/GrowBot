using System.Web.Mvc;

namespace GrowBot.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.User = System.Web.HttpContext.Current.User.Identity.Name;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.User = System.Web.HttpContext.Current.User.Identity.Name;
            return View();
        }

        [Authorize]
        [Authorize(Roles = "Administrator")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            ViewBag.User = User.IsInRole("Administrator");
            return View();
        }
    }
}