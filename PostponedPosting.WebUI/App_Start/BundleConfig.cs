using System.Web;
using System.Web.Optimization;

namespace PostponedPosting.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-2.1.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-datatables").Include(
                "~/Scripts/DataTables/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                        "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));
            
            //Local date creator
            bundles.Add(new ScriptBundle("~/helpers/localDateCreator").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/moment-timezone-with-data-2010-2020.min.js",
                "~/Scripts/Custom/LocalDateCreator.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/toastr.min.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-datatables").Include(
                "~/Content/DataTables/css/jquery.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                     "~/Content/bootstrap-datetimepicker.css"));

            //Pages

            bundles.Add(new ScriptBundle("~/bundles/manage-posts").Include(
                "~/Scripts/Custom/Common.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/locales/bootstrap-datepicker.en-GB.min.js",
                "~/Scripts/Custom/MangePosting.js"));

            bundles.Add(new ScriptBundle("~/custom/ManageSNs/scripts").Include(
                "~/Scripts/Custom/ManageSN.js"));

            bundles.Add(new ScriptBundle("~/custom/manageGroupsOfLinks").Include(
                "~/Scripts/Custom/ManageLinksGroups.js"));

            bundles.Add(new ScriptBundle("~/custom/manageLinks").Include(
                "~/Scripts/Custom/Common.js",
                "~/Scripts/Custom/ManageLinks.js"));                 
        }
    }
}
