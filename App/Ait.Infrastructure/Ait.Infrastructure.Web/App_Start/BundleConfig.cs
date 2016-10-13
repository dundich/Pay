using System.Web.Optimization;

namespace Ait.Infrastructure.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/styles")
                .IncludeDirectory("~/Assets/app", "*.css", true)
                .Include("~/content/loading-bar.min.css")
                );

            bundles.Add(new StyleBundle("~/content/fontawesome/css/fa")
                .Include("~/content/fontawesome/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/content/materialize/css/ma")
                .Include("~/content/materialize/css/materialize.css"));


            bundles.Add(new ScriptBundle("~/ng")
                .Include("~/Scripts/angular.min.js"
                        , "~/Scripts/angular-route.min.js"
                        , "~/Scripts/angular-animate.min.js"
                        , "~/Scripts/angular-sanitize.min.js"
                        , "~/Scripts/angular-cookies.min.js"
                        , "~/Scripts/loading-bar.min.js"
                        , "~/Scripts/ngStorage.min.js"
                        ));


            bundles.Add(new ScriptBundle("~/ma")
                .Include("~/Scripts/materialize/materialize.js"));

            bundles.Add(new ScriptBundle("~/app")
                .IncludeDirectory("~/Assets/app", "*.js", true));


            bundles.Add(new ScriptBundle("~/jquery")
                .Include(
                        "~/Scripts/jquery-2.1.1.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.bundle.js"
                )
                .IncludeDirectory("~/Assets/jquery", "*.js", true));


            bundles.Add(new ScriptBundle("~/misc")
                .IncludeDirectory("~/Assets/misc", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862




            //BundleTable.EnableOptimizations = true;
        }
    }
}
