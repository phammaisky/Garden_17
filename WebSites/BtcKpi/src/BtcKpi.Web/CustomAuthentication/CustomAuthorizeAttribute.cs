using System.Web;
using System.Web.Mvc;

namespace BtcKpi.Web
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var currentUser = httpContext.User.Identity.Name;
            string role = request.Url.AbsolutePath.Remove(0, 1); ;
            if (string.IsNullOrEmpty(role))
            {
                string controller = request.RequestContext.RouteData.Values["controller"].ToString();
                string action = request.RequestContext.RouteData.Values["action"].ToString();
                role = string.Format("{0}/{1}", controller, action);
            }

            return ((CurrentUser != null && !CurrentUser.IsInRole(role)) || CurrentUser == null) ? false : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if(CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Account",
                        action = "Login",
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }

    }
}