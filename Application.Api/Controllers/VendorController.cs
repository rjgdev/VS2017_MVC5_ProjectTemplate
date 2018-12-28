using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Newtonsoft.Json;

namespace Application.Api.Controllers
{
    /// <summary>
    ///     Vendor API
    /// </summary>
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/Vendor")]
    public class VendorController : BaseApiController
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        //[HttpGet]
        //[Route("GetList/{take}")]
        //public async Task<IHttpActionResult> GetList(int take)
        //{
        //    var vendor = await _vendorService.GetListAsync(take);
        //    if (vendor == null)
        //        return Content(HttpStatusCode.NotFound, $"No vendor record were found.");

        //    return Ok(vendor);
        //}

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IHttpActionResult> GetById(long id)
        {
            var vendor = await _vendorService.GetByIdAsync(id);
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"Vendor ID [{id}] not found.");

            return Ok(vendor);
        }


        [HttpPost]
        [Route("Add")]
        // POST api/<controller>
        public IHttpActionResult Post(object obj)
        {
            try
            {
                var retId = _vendorService.Add(obj);

                bool IsDuplicate = retId == 0;
                if (IsDuplicate == true)
                {
                    Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Add record not successful, Vendor Name is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Vendor Name is Duplicate");
                }

                Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Add record successful.");

                var response = Request.CreateResponse(HttpStatusCode.Created);
                var test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Vendor added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                Log.Error(typeof(VendorController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [HttpPost]
        [Route("AddAsync")]
        // POST api/<controller>
        public async Task<IHttpActionResult> PostAsync(object obj)
        {
            try
            {
                var retId = await _vendorService.AddAsyc(obj);

                if (retId == 0)
                {
                    Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Add record not successful, Vendor Name is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Vendor Name is Duplicate");
                }
                
                Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Add record successful.");

                var response = Request.CreateResponse(HttpStatusCode.Created);
                var test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Vendor added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                Log.Error(typeof(VendorController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        // PUT api/<controller>/5
        public IHttpActionResult Put(object obj)
        {
         
            bool IsNotDuplicate = _vendorService.Update(obj);
            if (IsNotDuplicate == true)
            {
                Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "Vendor record was updated successfully.");
            }
           
            Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Update record not successful, Vendor Name is duplicate.");
            return Content(HttpStatusCode.Forbidden, "Vendor Name is Duplicate");
        }

        /// <summary>
        /// Update vendor record (Async)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateAsync")]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> PutAsync(object obj)
        {
            await _vendorService.UpdateAsync(obj);
            Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Update record successful.");

            return Content(HttpStatusCode.OK, "Vendor record was updated successfully.");
        }

        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _vendorService.Delete(id,updatedBy);
                Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "Vendor record deleted");
            }
            catch (Exception ex)
            {
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
                _vendorService.Enable(id,updatedBy);
                Log.Info($"{typeof(VendorController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Vendor record enabled");
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
                var obj = _vendorService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _vendorService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}