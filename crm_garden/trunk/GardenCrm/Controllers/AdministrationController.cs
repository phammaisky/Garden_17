using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenCrm.CustomAuthentication;

namespace GardenCrm.Controllers
{
    [CustomAuthorizeMembership(Roles = "Administration")]
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }
    }
}