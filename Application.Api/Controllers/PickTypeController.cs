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
    [RoutePrefix("api/PickType")]
    public class PickTypeController : BaseApiController
    {
        private readonly IPickTypeService _pickTypeService;

        public PickTypeController(IPickTypeService pickTypeService)
        {
            _pickTypeService = pickTypeService;
        }

        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var vendor = _pickTypeService.GetList(take);
        //    if (vendor == null)
        //        return Content(HttpStatusCode.NotFound, $"No status record were found.");

        //    return Ok(vendor);
        //}

        [HttpGet]
        [RequireHttps]
        [Route("GetSelectList")]
        public IHttpActionResult GetSelectList()
        {
            var vendor = _pickTypeService.GetAll();
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"No pick type record were found.");

            return Ok(vendor);
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var vendor = _pickTypeService.GetById(id);
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"Pick Type ID [{id}] not found.");

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
        public IHttpActionResult Post(PickType obj)
        {
            try 
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                long retId = _pickTypeService.Add(obj);

                if (retId == 0)
                {
                    Log.Info($"{typeof(PickTypeController).FullName}||{UserEnvironment}||Add record not successful, Pick Type Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Pick Type Code is Duplicate");
                }
                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Pick Type added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                Log.Info($"{typeof(PickType).FullName}||{UserEnvironment}||Add record successful.");

                return ResponseMessage(response);
              
            }
            catch (Exception e)
            {
                Log.Error(typeof(PickTypeController).FullName, e);

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
        public IHttpActionResult Put(PickType obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool IsNotDuplicate = _pickTypeService.Update(obj);
            if (IsNotDuplicate == true)
            {
              
                Log.Info($"{typeof(PickTypeController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "Pick Type record was updated successfully.");
            }
            Log.Info($"{typeof(PickTypeController).FullName}||{UserEnvironment}||Update record not successful, Pick Type Code is duplicate.");
            return Content(HttpStatusCode.Forbidden, "Pick Type Code is Duplicate");


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
                _pickTypeService.Delete(id,updatedBy);
                Log.Info($"{typeof(PickTypeController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "Pick Type record was deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///     Deletes the record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Enable/{id}/{updatedBy}")]
        // Enable api/<controller>/5
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _pickTypeService.Enable(id,updatedBy);
                Log.Info($"{typeof(PickTypeController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Pick Type record was enabled.");
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
                var obj = _pickTypeService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _pickTypeService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}