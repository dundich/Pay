using System.Web.Optimization;

namespace Ait.Infrastructure.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new StyleBundle("~/styles").IncludeDirectory("~/Assets/app", "*.css", true));


            bundles.Add(new ScriptBundle("~/ng")
                .Include("~/Scripts/angular.min.js"
                        , "~/Scripts/angular-route.min.js"
                        , "~/Scripts/angular-animate.min.js"
                        , "~/Scripts/angular-sanitize.min.js"
                        , "~/Scripts/angular-cookies.min.js"
                        , "~/Scripts/ngStorage.min.js"
                        ));


            bundles.Add(new ScriptBundle("~/app").IncludeDirectory("~/Assets/app", "*.js", true));


            bundles.Add(new ScriptBundle("~/jquery")
                .Include(
                        "~/Scripts/jquery/jquery-2.1.1.min.js",
                        "~/Scripts/jquery.inputmask/inputmask.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask/inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
                        "~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js")
                .IncludeDirectory("~/Assets/jquery", "*.js", true));


            bundles.Add(new ScriptBundle("~/misc").IncludeDirectory("~/Assets/misc", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
