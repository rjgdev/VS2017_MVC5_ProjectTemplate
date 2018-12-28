using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Newtonsoft.Json;

namespace Application.Api.Controllers
{
    /// <summary>
    ///     Hauliers API
    /// </summary>
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/Haulier")]
    public class HauliersController : BaseApiController
    {
        private readonly IHaulierService _haulierService;

        public HauliersController(IHaulierService haulierService)
        {
            _haulierService = haulierService;
        }

        //[HttpGet]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var obj = _haulierService.GetList(take);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No haulier record were found.");

        //    return Ok(obj);
        //}

        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var obj = _haulierService.GetById(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"Haulier ID [{id}] not found.");

            return Ok(obj);
        }


        [HttpPost]
        [Route("Add")]
        // POST api/<controller>
        public IHttpActionResult Post(object obj)
        {
            try
            {
                var retId = _haulierService.Add(obj);

                if (retId == 0)
                {
                    Log.Info($"{typeof(HauliersController).FullName}||{UserEnvironment}||Add record not successful, Haulier Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Haulier Code is Duplicate");
                }
                var response = Request.CreateResponse(HttpStatusCode.Created);
                var test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Haulier added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                Log.Info($"{typeof(HauliersController).FullName}||{UserEnvironment}||Add record successful.");
                return ResponseMessage(response);
                //return Content(HttpStatusCode.Accepted, "Accepted but with error.");
            }
            catch (DbEntityValidationException ex)
            {
                dynamic errlist = new
                {
                    Message = "Validation error",
                    ErrorList = (from err in ex.EntityValidationErrors from error in err.ValidationErrors select error.ErrorMessage).Cast<object>().ToList()
                };

                Log.Error(typeof(HauliersController).FullName, ex);

                return Content(HttpStatusCode.BadRequest, errlist);
            }
            catch (Exception e)
            {
                Log.Error(typeof(CustomerClientController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        // PUT api/<controller>/5
        public IHttpActionResult Put(object obj)
        {
            try
            {
                 bool isNotDuplicate =  _haulierService.Update(obj);
                if (isNotDuplicate == true)
                {

                    Log.Info($"{typeof(HauliersController).FullName}||{UserEnvironment}||Update record successful.");
                    return Content(HttpStatusCode.OK, "Haulier record was updated successfully.");

                }
                Log.Info($"{typeof(HauliersController).FullName}||{UserEnvironment}||Update record not successful, Haulier Code is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Haulier Code is Duplicate");

            }
            catch (DbEntityValidationException ex)
            {
                dynamic errlist = new
                {
                    Message = "Validation error",
                    ErrorList = (from err in ex.EntityValidationErrors from error in err.ValidationErrors select error.ErrorMessage).Cast<object>().ToList()
                };

                Log.Error(typeof(CustomerClientController).FullName, ex);

                return Content(HttpStatusCode.BadRequest, errlist);
            }
            catch (Exception e)
            {
                Log.Error(typeof(CustomerClientController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _haulierService.Delete(id,updatedBy);
                Log.Info($"{typeof(HauliersController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Haulier record deleted");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(CustomerClientController).FullName, ex);

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("Enable/{id}/{updatedBy}")]
        // ENABLE api/<controller>/5
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _haulierService.Enable(id,updatedBy);
                Log.Info($"{typeof(HauliersController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Haulier record enabled");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(CustomerClientController).FullName, ex);

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetSelectList")]
        public IHttpActionResult GetSelectList()
        {
            var obj = _haulierService.GetAll();
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"Haulier List empty");

            return Ok(obj);
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
                var obj = _haulierService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                 var obj = _haulierService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}