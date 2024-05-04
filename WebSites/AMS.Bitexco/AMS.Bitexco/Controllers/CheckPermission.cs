using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class CheckPermission
    {
        public static UserInfo CheckControler(Controller ctl, string UserAccount, out GroupUser_Authorize grAu)
        {
            grAu = null;
            string Control = ctl.ControllerContext.RouteData.Values["controller"].ToString();
            string Action = ctl.ControllerContext.RouteData.Values["action"].ToString();

            string ControlerAction = Control + "/" + Action;
            using (AMSEntities db = new AMSEntities())
            {
                var user = db.UserInfoes.Where(u => u.UserName.ToLower() == UserAccount.ToLower() && u.IsLock==false).FirstOrDefault();
                if (user != null)
                {
                    var grOfUser = user.GroupUsers.Where(g => g.AppName == "AMS").FirstOrDefault();
                    if (grOfUser != null)
                     {
                         var groupUser = db.GroupUsers.Where(gr => gr.Id == grOfUser.Id).FirstOrDefault();
                         if (groupUser != null)
                         {
                             grAu = groupUser.GroupUser_Authorize.Where(a => a.Authorize.UrlControlAction == ControlerAction).FirstOrDefault();

                         }
                     }
                    return user;
                }
            }
            return null;
        }

        public static UserInfo CheckControler(string Control, string Action, string UserAccount, out GroupUser_Authorize grAu)
        {
            grAu = null;
            string ControlerAction = Control + "/" + Action;
            using (AMSEntities db = new AMSEntities())
            {
                var user = db.UserInfoes.Where(u => u.UserName.ToLower() == UserAccount.ToLower() && u.IsLock == false).FirstOrDefault();
                if (user != null)
                {
                    var grOfUser = user.GroupUsers.Where(g => g.AppName == "AMS").FirstOrDefault();
                    if (grOfUser != null)
                    {
                        var groupUser = db.GroupUsers.Where(gr => gr.Id == grOfUser.Id).FirstOrDefault();
                        if (groupUser != null)
                        {
                            grAu = groupUser.GroupUser_Authorize.Where(a => a.Authorize.UrlControlAction == ControlerAction).FirstOrDefault();

                        }
                    }
                    return user;
                }
            }
            return null;
        }
        public static bool CheckUserSession(string _userName)
        {
            using (AMSEntities db = new AMSEntities())
             {
                 if (db.UserInfoes.Where(u => u.UserName.ToLower() == _userName.ToLower()).FirstOrDefault() != null)
                     return true;
                 else
                     return false;
             }
        }

        public static UserInfo ReturnUserSession(string _userName)
        {
            using (AMSEntities db = new AMSEntities())
            {
                var user = db.UserInfoes.Where(u => u.UserName.ToLower() == _userName.ToLower() && u.IsLock == false).FirstOrDefault();
                return user;
            }
        }

       
    }
}