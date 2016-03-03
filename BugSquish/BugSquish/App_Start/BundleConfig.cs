using System.Web;
using System.Web.Optimization;

namespace BugSquish
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            //jquery scripts
            bundles.Add(new ScriptBundle("~/Assets/js/jquery").Include(
                        "~/jquery/jquery-1.10.2.min.js",
                        "~/jquery/jquery-ui 1.10.3.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));


            //chart scripts
            bundles.Add(new ScriptBundle("~/Assets/js/chart").Include(
                "~/chart/highchart.js",
                "~/chart/modules/exporting.js"));


            //other scripts
            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                      "~/js/bootstrap.min.js",
                      "~/js/feeds.js",
                      "~/js/chosen.js",
                      "~/js/shadowbox.js",
                      "~/js/sparkline.min.js",
                      "~/js/masonry.min.js",
                      "~/js/calendar.min.js",
                      "~/js/datatables.min.js",
                      "~/js/autosize.min.js",
                      "~/js/select.min.js",
                      "~/js/toggler.min.js",
                      "~/js/datetimepicker.min.js",
                      "~/js/colorpicker.min.js",
                      "~/js/fileupload.min.js",
                      "~/js/media-player.js",
                      "~/js/validation/validate.min.js",
                      "~/js/validation/validationEngine.min.js",
                      "~/js/functions.js"
                      ));

            //style scripts
            bundles.Add(new StyleBundle("~/Assets/css").Include(
                      "~/css/bootstrap.min.css",
                      "~/css/calendar.min.css",
                      "~/css/icomoon.min.css",
                      "~/css/media-player.min.css",
                      "~/css/file-manager.min.css",
                      "~/css/form.min.css",
                      "~/css/style.min.css"));

        
        
        
        }
    }
}
