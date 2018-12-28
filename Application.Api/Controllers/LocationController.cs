using System;
using System.Net;
using System.Web.Http;
using Application.Bll;
using Application.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Application.Api.Filters;

namespace Application.Api.Controllers
{
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/Location")]
    public class LocationController : BaseApiController
    {
        private readonly ILocationService _locationService;


        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        ///     Gets the location by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult Get(int id)
        {
            var location = _locationService.GetLocationById(id);
            if (location == null)
                return Content(HttpStatusCode.NotFound, $"Location ID [{id}] not found.");

            return Ok(location);
        }

        //[HttpGet]
        //[Route("GetDynamicList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var customer = _locationService.GetDynamicList(take);
        //    if (customer == null)
        //        return Content(HttpStatusCode.NotFound, $"Customer list empty");

        //    return Ok(customer);
        //}

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var locations = _locationService.GetAll();
            if (locations == null)
                return Content(HttpStatusCode.NotFound, $"Customer list empty");

            return Ok(locations);
        }

        /// <summary>
        /// Get By WarehouseCode
        /// </summary>
        /// <param name="warehouseCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByWarehouseCode/{warehouseCode}/{isActive}/{customerId}")]
        public IHttpActionResult GetList(string warehouseCode, bool isActive, long customerId)
        {
            var customer = _locationService.GetByWarehouseCode(warehouseCode, isActive, customerId);
            if (customer == null)
                return Content(HttpStatusCode.NotFound, $"Customer list empty");

            return Ok(customer);
        }

        /// <summary>
        /// Adds the location record.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Post(Location location)
        {
            long retId;
            try
            {
                if (location == null) throw new ArgumentException("Location model is null.");

                retId = _locationService.Add(location);
                if (retId == 0)
                {
                    Log.Info($"{typeof(LocationController).FullName}||{UserEnvironment}||Add record not successful, Location Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Location Code is Duplicate");
                }

                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Location added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                Log.Info($"{typeof(LocationController).FullName}||{UserEnvironment}||Add record successful.");

                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                Log.Error(typeof(LocationController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

           


        }

        /// <summary>
        /// Update the record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update")]
        // PUT api/<controller>/5
        public IHttpActionResult Put(Location obj)
        {
           
            bool IsNotDuplicate = _locationService.Update(obj);
            if (IsNotDuplicate == true)
            {
                Log.Info($"{typeof(LocationController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "Location record was updated successfully.");
            }
           
            Log.Info($"{typeof(LocationController).FullName}||{UserEnvironment}||Update record not successful, Location Code is duplicate.");
            return Content(HttpStatusCode.Forbidden, "Location Code is Duplicate");

        }

        /// <summary>
        /// Deletes the record
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
                _locationService.Delete(id,updatedBy);
                Log.Info($"{typeof(LocationController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Location record was deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes the record
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
                _locationService.Enable(id,updatedBy);
                Log.Info($"{typeof(LocationController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Location record was enabled.");
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
                var obj = _locationService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _locationService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}