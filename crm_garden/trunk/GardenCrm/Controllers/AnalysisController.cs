using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenCrm.CustomAuthentication;

namespace GardenCrm.Controllers
{
    [CustomAuthorizeMembership(Roles = "Analysis")]
    public class AnalysisController : Controller
    {
        // GET: Analysis
        public ActionResult Index()
        {
            return View();
        }
    }
}