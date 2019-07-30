using System.Web;
using System.Web.Optimization;

namespace SpointLiteVersion
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

            bundles.Add(new ScriptBundle("~/Js").Include(
                      "~/Content/bower_components/jquery/dist/jquery.min.js",
                      "~/Scripts/jquery.unobtrusive-ajax.min.js",
                                       

            "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/Content/bower_components/fastclick/lib/fastclick.js",
                      "~/Content/dist/js/adminlte.min.js",
                      "~/Content/dist/js/demo.js"
));

            bundles.Add(new StyleBundle("~/Css").Include(
                      "~/Content/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                       "~/Content/dist/css/skins/_all-skins.min.css",
                      "~/Content/Style.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
