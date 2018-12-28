using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Application.Api.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            #if !DEBUG
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            else
            #endif
                base.OnAuthorization(actionContext);
        }
    }
}
