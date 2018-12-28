using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Api;
using Application.Api.Filters;
using log4net;

namespace Application.Api.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private readonly ILog _log = Logger.LoggingInstance;

        // GET api/values
        [RequireHttps]
        public IEnumerable<string> Get()
        {
            _log.Info("Returned the requested values.");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(long id)
        {
        }
    }
}
