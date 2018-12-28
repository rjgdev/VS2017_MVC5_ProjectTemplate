using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace Application.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css")
                .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/bootstrap-select.css")
                .Include("~/Content/css/bootstrap-datepicker3.min.css")
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/icheck/blue.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/AdminLTE.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/skins/skin-blue.css")
                .Include("~/Content/DataTables/datatables.min.css")
                .Include("~/Content/DataTables/references/css/dataTables.bootstrap..css")
                .Include("~/Content/DataTables/references/css/dataTables.bootstrap4.css")
                .Include("~/Content/DataTables/references/css/dataTables.foundation.css")
                .Include("~/Content/DataTables/references/css/dataTables.jqueryui.css")
                .Include("~/Content/DataTables/references/css/dataTables.semanticui.css")
                .Include("~/Content/DataTables/references/css/jquery.dataTables.css")
                );

            bundles.Add(new ScriptBundle("~/Bundles/js")
                .Include("~/Content/js/plugins/jquery/jquery-3.3.1.js")
                .Include("~/Content/js/plugins/bootstrap/bootstrap.js")
                .Include("~/Content/js/plugins/fastclick/fastclick.js")
                .Include("~/Content/js/plugins/slimscroll/jquery.slimscroll.js")
                .Include("~/Content/js/plugins/bootstrap-select/bootstrap-select.js")
                .Include("~/Content/js/plugins/moment/moment.js")
                .Include("~/Content/js/plugins/datepicker/bootstrap-datepicker.js")
                .Include("~/Content/js/plugins/icheck/icheck.js")
                .Include("~/Content/js/plugins/validator.js")
                .Include("~/Content/js/plugins/inputmask/jquery.inputmask.bundle.js")
                .Include("~/Content/js/adminlte.js")
                .Include("~/Content/js/init.js")
                .Include("~/Content/DataTables/datatables.min.js")
                .Include("~/Content/DataTables/references/js/dataTables.bootstrap.min.js")
                .Include("~/Content/DataTables/references/js/dataTables.bootstrap4.min.js")
                .Include("~/Content/DataTables/references/js/dataTables.foundation.min.js")
                .Include("~/Content/DataTables/references/js/dataTables.jqueryui.min.js")
                .Include("~/Content/DataTables/references/js/dataTables.semanticui.min.js")
                .Include("~/Content/DataTables/references/js/jquery.dataTables.min.js")
                //.Include("~/Content/js/plugins/jquery/jQuery-Validation-Engine.js")
                //.Include("~/Content/js/plugins/jquery/jQuery.validationEngine-en.js")
                .Include("~/Content/js/plugins/jquery/jquery.validate.min.js")
                .Include("~/Content/js/plugins/jquery/jquery.validate.unobtrusive.min.js")
                .Include("~/Content/SweetAlert/sweetalert.min.js"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
