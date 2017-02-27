using PostponedPosting.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PostponedPosting.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<DataContext>(new AppDbInitializer());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        //internal protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    // Get objects.
        //    HttpContext context = base.Context;
        //    HttpResponse response = context.Response;
        //    // Complete.
        //   // base.CompleteRequest();
        //}
    }
}
