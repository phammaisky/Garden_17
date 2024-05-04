using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenCrm.CustomAuthentication;

namespace GardenCrm.Controllers
{
    [CustomAuthorizeMembership(Roles = "Sale")]
    public class SaleController : Controller
    {
        // GET: Sale
        public ActionResult Index()
        {
            return View();
        }
    }
}