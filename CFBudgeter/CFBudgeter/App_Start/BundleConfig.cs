using System.Web;
using System.Web.Optimization;

namespace CFBudgeter
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            bundles.UseCdn = true;
            var googleFontPath = "http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,800italic,400,600,800";


            bundles.Add(new StyleBundle("~/Assets/css", 
                googleFontPath).Include(
                       
                      "~/css/bootstrap.min.css",
                      "~/js/libs/css/ui-lightness/jquery-ui-1.9.2.custom.css",
                      "~/css/Apps.css",
                      "~/css/custom.css"));

            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                      "~/js/libs/jquery-1.9.1.min.js",
                      "~/js/libs/jquery-ui-1.9.2.custom.min.js",
                      "~/js/libs/bootstrap.min.js",
                      "~/js/App.js"));
        }
    }
}
