using System;
using System.Net;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Application.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Application.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Status")]
    public class StatusController : BaseApiController
    {
        private readonly IStatusService _statusService;
        
        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var vendor = _statusService.GetList(take);
        //    if (vendor == null)
        //        return Content(HttpStatusCode.NotFound, $"No status record were found.");

        //    return Ok(vendor);
        //}

        [HttpGet]
        [RequireHttps]
        [Route("GetSelectList")]
        public IHttpActionResult GetSelectList()
        {
            var vendor = _statusService.GetAll();
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"No status record were found.");

            return Ok(vendor);
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var vendor = _statusService.GetById(id);
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"Status ID [{id}] not found.");

            return Ok(vendor);
        }

        /// <summary>
        ///     Adds the status record.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        public IHttpActionResult Post(object obj)
        {
            try
            {

                long retId = _statusService.Add(obj);

                if (retId == 0)
                {
                    Log.Info($"{typeof(StatusController).FullName}||{UserEnvironment}||Add record not successful, Status Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Status Code is Duplicate");
                }


                Log.Info($"{typeof(StatusController).FullName}||{UserEnvironment}||Add record successful.");

                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Status added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                Log.Error(typeof(StatusController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

        }

        /// <summary>
        ///     Update the record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update")]
        // PUT api/<controller>/5
        public IHttpActionResult Put(object obj)
        {
          
            bool IsNotDuplicate = _statusService.Update(obj);
            if (IsNotDuplicate == true)
            {
                Log.Info($"{typeof(StatusController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "Status record was updated successfully.");
            }
           

            Log.Info($"{typeof(StatusController).FullName}||{UserEnvironment}||Update record not successful, Status Code is duplicate.");
            return Content(HttpStatusCode.Forbidden, "Status Code is Duplicate");
        }

        /// <summary>
        ///     Deletes the record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _statusService.Delete(id,updatedBy);

                Log.Info($"{typeof(StatusController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "Status record was deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        
        /// <summary>
        ///     Enables the record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Enable/{id}/{updatedBy}")]
        // ENABLE api/<controller>/5
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _statusService.Enable(id,updatedBy);

                Log.Info($"{typeof(StatusController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Status record was enabled.");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetList/{isActive}/{CustomerId}/{pageNo?}/{pageSize?}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetList(bool isActive, long customerId, int? pageNo = null, int? pageSize = null)
        {

            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _statusService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _statusService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}