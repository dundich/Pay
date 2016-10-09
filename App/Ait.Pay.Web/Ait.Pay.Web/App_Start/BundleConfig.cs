using System.Web.Optimization;

namespace Ait.Pay.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            //bundles.Add(new ScriptBundle("~/bootstrap").Include(
            //         "~/Assets/Bootstrap/js/bootstrap.js"));


            //bundles.Add(new ScriptBundle("~/materialize").Include(
            //         "~/Content/materialize-v0.97.5/js/materialize.min.js"));


            //Moved bootstrap css to own style tag in the _layout page.  This is to remove it from the optimizations which was breaking the fonts and icons.

            bundles.Add(new StyleBundle("~/styles").IncludeDirectory("~/Assets/app", "*.css", true));


            bundles.Add(new ScriptBundle("~/ng")
                .Include("~/Scripts/angular.min.js"
                        , "~/Scripts/angular-route.min.js"
                        , "~/Scripts/angular-animate.min.js"
                        , "~/Scripts/angular-sanitize.min.js"
                        , "~/Scripts/angular-cookies.min.js"
                        , "~/Scripts/ngStorage.min.js"
                        //, "~/Scripts/select.js"

                        ));

            bundles.Add(new ScriptBundle("~/app").IncludeDirectory("~/Assets/app", "*.js", true));


            bundles.Add(new ScriptBundle("~/jquery")
                .Include("~/Assets/jquery/jquery.min.js"
                        , "~/Scripts/jquery.inputmask/jquery.inputmask.bundle.js"
                //, "~/Scripts/select2.min.js"
                )
                .IncludeDirectory("~/Assets/jquery", "*.js", true));



            bundles.Add(new ScriptBundle("~/misc").IncludeDirectory("~/Assets/misc", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

        }
    }
}
