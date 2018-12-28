using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Application.Web.App_Start;
using Application.Web.Controllers;
using System.Web.Helpers;
using System.Security.Claims;

namespace Application.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //var exception = Server.GetLastError();
            //var httpContext = ((HttpApplication)sender).Context;
            //httpContext.Response.Clear();
            //httpContext.ClearError();

            //if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            //{
            //    return;
            //}

            //ExecuteErrorController(httpContext, exception as HttpException);


            var exception = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;

            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 401:
                        routeData.Values["action"] = "Http401";
                        break;
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            IController errorsController = new ErrorController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        //private void ExecuteErrorController(HttpContext httpContext, HttpException exception)
        //{
        //    var routeData = new RouteData();
        //    routeData.Values["controller"] = "Error";

        //    if (exception != null && exception.GetHttpCode() == (int)HttpStatusCode.NotFound)
        //    {
        //        routeData.Values["action"] = "NotFound";
        //    }
        //    else
        //    {
        //        routeData.Values["action"] = "InternalServerError";
        //    }

        //    using (Controller controller = new ErrorController())
        //    {
        //        ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        //    }
        //}
    }
}