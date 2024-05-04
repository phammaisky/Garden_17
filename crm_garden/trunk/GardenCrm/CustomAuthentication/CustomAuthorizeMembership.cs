using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GardenCrm.CustomAuthentication
{
    public class CustomAuthorizeMembership : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
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