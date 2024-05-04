using _IQwinwin;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GardenLover
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            sys.Application_Start(null, null);
            new _IQ().Application_Start(null, null);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            sys.Application_Error(sender, e);
            new _IQ().Application_Error(sender, e);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            sys.Application_BeginRequest(sender, e);
            new _IQ().Application_BeginRequest(sender, e);
        }
    }
}
