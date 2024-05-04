using GardenLover.EF;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace GardenLover.Controllers
{
    public class HomeController : Controller
    {
        CompanyEntities dbCompany = new CompanyEntities();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View("Index");
        }
    }
}