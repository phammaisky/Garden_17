using System.Web.Optimization;

namespace GardenLover
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/Jquery").Include(
                        "~/Content/_Plugin/JQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Bundles/Jquery.Validate").Include(
                        "~/Content/_Plugin/JQuery.Validate/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/Bundles/Modernizr").Include(
                        "~/Content/_Plugin/OldBrowser/modernizr-*"));

            bundles.Add(new StyleBundle("~/Bundles/Bootstrap/CSS").Include(
                      "~/Content/_Plugin/Bootstrap/CSS/bootstrap.css",
                      "~/Content/_Plugin/Bootstrap/CSS/site.css"));

            bundles.Add(new ScriptBundle("~/Bundles/Bootstrap/JS").Include(
                      "~/Content/_Plugin/Bootstrap/JS/bootstrap.js",
                      "~/Content/_Plugin/OldBrowser/respond.js"));

            bundles.Add(new StyleBundle("~/Bundles/Bootstrap.DatePicker/CSS").Include(
                      "~/Content/_Plugin/Bootstrap.Datepicker/CSS/bootstrap-datepicker.css"));

            bundles.Add(new ScriptBundle("~/Bundles/Bootstrap.DatePicker/JS").Include(
                      "~/Content/_Plugin/Bootstrap.Datepicker/JS/bootstrap-datepicker.js"));

            bundles.Add(new StyleBundle("~/Bundles/Bootstrap.DatetimePicker/CSS").Include(
                      "~/Content/_Plugin/Bootstrap.DatetimePicker/CSS/bootstrap-datetimepicker.css",
                      "~/Content/_Plugin/Bootstrap.DatetimePicker/CSS/variables.less"));

            bundles.Add(new ScriptBundle("~/Bundles/Bootstrap.DatetimePicker/JS").Include(
                      "~/Content/_Plugin/Bootstrap.DatetimePicker/JS/moment.js",
                      "~/Content/_Plugin/Bootstrap.DatetimePicker/JS/bootstrap-datetimepicker.js"));

            //JsTree
            bundles.Add(new StyleBundle("~/Bundles/JsTree/CSS").Include(
                      "~/Content/_Plugin/JsTree/themes/default/style.css"));

            bundles.Add(new ScriptBundle("~/Bundles/JsTree/JS").Include(
                      "~/Content/_Plugin/JsTree/jstree.js"));

            //CSS | JS
            bundles.Add(new StyleBundle("~/Bundles/CSS").Include(
                      "~/Content/All.css"));

            bundles.Add(new ScriptBundle("~/Bundles/JS").Include(
                      "~/Content/JS.js",
                      "~/Content/Dialog.js"
                      ));            
        }
    }
}
