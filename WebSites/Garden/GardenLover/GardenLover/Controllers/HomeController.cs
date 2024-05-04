using _IQwinwin;
using IQWebApp_Blank.Models;
using IQWebApp_Blank.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQWebApp_Blank.Controllers
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