using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenCrm.CustomAuthentication;

namespace GardenCrm.Controllers
{
    [CustomAuthorizeMembership(Roles = "Customer")]
    public class CustomerController : CustomController
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Create Customer
        public ActionResult Create()
        {
            return View();
        }
    }
}