using System.Linq;
using System.Web;
using System.Web.Http;
using log4net;

namespace Application.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected readonly ILog Log = Logger.LoggingInstance;
       protected readonly string UserEnvironment = string.Format("MachineName::{0}", System.Web.HttpContext.Current.Server.MachineName);
    }
}