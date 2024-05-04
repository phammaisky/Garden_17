using System.Web.Mvc;
using AMS.Models;


namespace AMS.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult PermitError()
        {
            return View("PermitError");
        }
        public ActionResult PermitErrorPop()
        {
            return PartialView("_PermitErrorPop");
        }
        public ActionResult UserLogin()
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "DeviceAndTools");
            }

        }
    }
}