using System.Web.Mvc;

namespace BtcKpi.Web.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult DashboardV1()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DashboardV2()
        {
            return View();
        }

        public ActionResult Index()
        {
            var test = CurrentUser;
            return View();
        }
    }
}