using System.Web.Optimization;

namespace Ait.Pay.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/styles")
                .IncludeDirectory("~/Assets/app", "*.css", true)
                .IncludeDirectory("~/Assets/jquery", "*.css", true));

            bundles.Add(new ScriptBundle("~/app")
                .IncludeDirectory("~/Assets/app", "*.js", true));

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
