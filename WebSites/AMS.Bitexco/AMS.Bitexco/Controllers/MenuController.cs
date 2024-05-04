using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class MenuController : Controller
    {
        AMSEntities db = new AMSEntities();
        //
        // GET: /Menu/
        public ActionResult TopMenu()
        {          
            try
            {              
                var user = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                if (user != null)
                {
                    var gropUserId = user.GroupUsers.Where(g => g.AppName == "AMS").First().Id;
                    var menu = db.Database.SqlQuery<TopMenu>("TopMenu @groupUserId={0}", gropUserId.ToString());

                    List<Group<string, TopMenu>> listMenu = (from ps in menu 
                                                            group ps by ps.GroupMenuName into gps
                                                            select new Group<string, TopMenu> { Key = gps.Key, Values = gps }).ToList();

                    ViewBag.Menu = listMenu;
                }
            }
            catch
            {
                Session["userId"] = null;
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}