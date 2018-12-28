using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Bll;
using Application.Model;
using Newtonsoft.Json;
using System.Text;
using System.Web.Http.Description;
using Application.Api.Filters;

namespace Application.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("api/CustomerClient")]
    public class CustomerClientController : BaseApiController
    {
        private readonly ICustomerClientService _customerClientService;


        public CustomerClientController(ICustomerClientService customerClientService)
        {
            _customerClientService = customerClientService;
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var CustomerClient = _customerClientService.GetById(id);
            if (CustomerClient == null)
                return Content(HttpStatusCode.NotFound, $"CustomerClient Id [{id}] not found.");

            return Ok(CustomerClient);
        }


        //[HttpGet]
        //[Route("GetByDomain/{domain}")]
        //public IHttpActionResult GetByDomain(string domain)
        //{
        //    var CustomerClient = _customerClientService.GetByDomain(domain);
        //    if (CustomerClient == null)
        //        return Content(HttpStatusCode.NotFound, $"CustomerClient domain [{domain}] not found.");

        //    return Ok(CustomerClient);
        //}

        //[HttpGet]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var CustomerClient = _customerClientService.GetList(take);
        //    if (CustomerClient == null)
        //        return Content(HttpStatusCode.NotFound, $"CustomerClient list empty");

        //    return Ok(CustomerClient);
        //}

        [HttpGet]
        [RequireHttps]
        [Route("GetSelectList")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetSelectList()
        {
            var list = _customerClientService.GetAll();

            return Ok(list);
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(CustomerClient model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                long retId = _customerClientService.Add(model);

                bool IsDuplicate = retId == 0;
                if (IsDuplicate == true)
                {
                    Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Add record not successful,Customer Client Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Customer Client Code is Duplicate");
                }

                Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Add record successful.");
                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Customer Client added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                return ResponseMessage(response);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                dynamic errlist = new
                {
                    Message = "Validation error",
                    ErrorList = (from err in ex.EntityValidationErrors from error in err.ValidationErrors select error.ErrorMessage).Cast<object>().ToList()
                };

                Log.Error(typeof(CustomerClientController).FullName, ex);

                return Content(HttpStatusCode.BadRequest, errlist);
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(CustomerClient CustomerClient)
        {
            try
            {
                bool success = _customerClientService.Update(CustomerClient, out bool IsDuplicate);
                if (IsDuplicate == true)
                {

                    Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Update record not successful, Customer Client Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Customer rClient Code is Duplicate");
                }
                Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "CustomerClient updated successfully");



            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                dynamic errlist = new
                {
                    Message = "Validation error",
                    ErrorList = (from err in ex.EntityValidationErrors from error in err.ValidationErrors select error.ErrorMessage).Cast<object>().ToList()
                };

                Log.Error(typeof(CustomerClientController).FullName, ex);

                return Content(HttpStatusCode.BadRequest, errlist);
            }
            catch (Exception ex)
            {
                Log.Error(typeof(CustomerClientController).FullName, ex);
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _customerClientService.Delete(id, updatedBy);
                Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Item deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("Enable/{id}/{updatedBy}")]
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _customerClientService.Enable(id,updatedBy);
                Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Item enabled");
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
                var obj = _customerClientService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _customerClientService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);

                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}
