using BtcKpi.Data;
using BtcKpi.Web.App_Start;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using BtcKpi.Web.ViewModels;
using System.Configuration;

namespace BtcKpi.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Init database
            //Release
            System.Data.Entity.Database.SetInitializer<BtcKpiEntities>(null);

            ////Using seeddata
            //System.Data.Entity.Database.SetInitializer(new StoreSeedData());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Autofac and Automapper configurations
            Bootstrapper.Run();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[ConfigurationManager.AppSettings["AppCookieName"]];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);

                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);

                principal.UserId = serializeModel.UserId;
                principal.UserName = serializeModel.UserName;
                principal.FullName = serializeModel.FullName;
                principal.Roles = serializeModel.RoleName.ToArray<string>();

                HttpContext.Current.User = principal;
            }

        }
    }
}
