using System.Web;
using System.Web.Optimization;

namespace GardenCrm
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterLayout(bundles);

            RegisterPlugins(bundles);

            RegisterAccount(bundles);
            
        }

        private static void RegisterLayout(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/adminlte/plugins/fontawesome-free/css/all.min.css",
                "~/adminlte/plugins/summernote/summernote-bs4.min.css",
                "~/adminlte/css/adminlte.min.css"));

            bundles.Add(new StyleBundle("~/Content/MainCss").Include(
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
                "~/adminlte/js/adminlte.min.js",
                "~/adminlte/plugins/summernote/summernote-bs4.min.js"));

        }

        private static void RegisterShared(BundleCollection bundles)
        {

        }

        private static void RegisterPlugins(BundleCollection bundles)
        {
            // plugins | Calendar
            bundles.Add(new StyleBundle("~/Calendar/css").Include(
                "~/adminlte/plugins/fullcalendar/main.min.css",
                "~/adminlte/plugins/fullcalendar-daygrid/main.min.css",
                "~/adminlte/plugins/fullcalendar-bootstrap/main.min.css"
            ));
            bundles.Add(new ScriptBundle("~/Calendar/js").Include(
                "~/adminlte/plugins/fullcalendar/main.js",
                "~/adminlte/plugins/fullcalendar-daygrid/main.js",
                "~/adminlte/plugins/fullcalendar-bootstrap/main.js"
            ));

            // plugins | icheck
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck-bootstrap/css").Include(
                "~/adminlte/plugins/icheck-bootstrap/icheck-bootstrap.css"));
        }

        private static void RegisterAccount(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Account/Login").Include(
                "~/Scripts/Account/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Account/Register").Include(
                "~/Scripts/Account/Register.js"));
        }
    }
}
