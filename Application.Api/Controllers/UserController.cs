using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Application.Api.Controllers
{

    [RequireHttps]
    [System.Web.Http.RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {

    }
}
