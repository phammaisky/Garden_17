using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IQWebApp_Blank.Startup))]
namespace IQWebApp_Blank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
