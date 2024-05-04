using System.Web.Mvc;

namespace GardenCrm.Controllers
{
    public class ErrorController : CustomController
    {
        // GET: Error
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}