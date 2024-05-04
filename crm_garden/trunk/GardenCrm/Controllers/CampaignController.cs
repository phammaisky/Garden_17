using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenCrm.CustomAuthentication;

namespace GardenCrm.Controllers
{
    [CustomAuthorizeMembership(Roles = "Campaign")]
    public class CampaignController : Controller
    {
        // GET: Campaign
        public ActionResult Index()
        {
            return View();
        }
    }
}