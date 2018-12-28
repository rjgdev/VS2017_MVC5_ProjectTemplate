using System;
using System.Net;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;

namespace Application.Api.Controllers
{
    /// <summary>
    /// Logs API
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Logs")]
    public class EventLogsController : ApiController
    {
        private readonly IEventLogService _eventLogService;

        public EventLogsController(IEventLogService eventLogService)
        {
            _eventLogService = eventLogService;
        }

        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var logs = _eventLogService.GetList(take);
        //    if (logs == null)
        //        return Content(HttpStatusCode.NotFound, $"No log record were found.");

        //    return Ok(logs);
        //}

        [HttpGet]
        [RequireHttps]
        [Route("GetList/{page}/{pageSize}")]
        public IHttpActionResult GetList(int page,int pageSize)
        {
            var logs = _eventLogService.GetList(page,pageSize);
            if (logs == null)
                return Content(HttpStatusCode.NotFound, $"No log record were found.");

            return Ok(logs);
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetList/{logType}/{page}/{pageSize}")]
        public IHttpActionResult GetList(string logType, int page, int pageSize)
        {
            var logs = _eventLogService.GetList(logType, page, pageSize);
            if (logs == null)
                return Content(HttpStatusCode.NotFound, $"No log record were found.");

            return Ok(logs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFrom">MM-dd-yyyy</param>
        /// <param name="dateTo">MM-dd-yyyy</param>
        /// <param name="logType">ERROR | INFO | DEBUG | FATAL</param>
        /// <param name="page">Default = 1</param>
        /// <param name="pageSize">Default = 10</param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetList/{dateFrom}/{dateTo}/{logType}/{page}/{pageSize}")]
        public IHttpActionResult GetList(string dateFrom, string dateTo, string logType, int page, int pageSize)
        {
            try
            {
                var dtFrom = DateTime.Parse(dateFrom);
                var dtTo = DateTime.Parse(dateTo);

                var logs = _eventLogService.GetListByDateRange(dtFrom, dtTo, logType, page, pageSize);
                if (logs == null)
                    return Content(HttpStatusCode.NotFound, $"No log record were found.");

                return Ok(logs);
            }
            catch (Exception exception)
            {
                return Content(HttpStatusCode.NotFound, "");
            }
        }

    }
}