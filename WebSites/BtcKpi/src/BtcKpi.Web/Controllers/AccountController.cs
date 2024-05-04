using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BtcKpi.Web.ViewModels;
using Newtonsoft.Json;

namespace BtcKpi.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginView, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            FullName = user.FullName,
                            RoleName = user.RolesFunctions.Select(r => r.FuncController + "/" + r.FuncAction + "-" + Convert.ToInt32(r.CanAdd) + "-" + Convert.ToInt32(r.CanView) + "-" + Convert.ToInt32(r.CanEdit) + "-" + Convert.ToInt32(r.CanDelete) + "-" + Convert.ToInt32(r.CanComment) + "-" + Convert.ToInt32(r.CanApprove)).ToList()
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                        (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CookieTimeout"])), false, userData
                        );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(ConfigurationManager.AppSettings["AppCookieName"], enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index","Home");
                    }
                }
            }
            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu. Vui lòng kiểm tra lại!");
            return View(loginView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            HttpCookie cookie = new HttpCookie(ConfigurationManager.AppSettings["AppCookieName"], "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        [HttpGet]
        public ActionResult LogOff(string userName)
        {
            Session.Clear();
            HttpCookie cookie = new HttpCookie(ConfigurationManager.AppSettings["AppCookieName"], "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

    }
}