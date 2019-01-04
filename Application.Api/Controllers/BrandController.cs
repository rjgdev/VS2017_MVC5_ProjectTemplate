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
    [RoutePrefix("api/Brand")]
    public class BrandController : BaseApiController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        /// <summary>
        ///     Gets the Brand by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult Get(int id)
        {
            var location = _brandService.GetById(id);
            if (location == null)
                return Content(HttpStatusCode.NotFound, $"Brand ID [{id}] not found.");

            return Ok(location);
        }

        /// <summary>
        ///     Gets the Brands by PRoduct ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByProductId/{id}")]
        public IHttpActionResult GetByProductId(int id)
        {
            var brands = _brandService.GetByProductId(id);
            if (brands == null)
                return Content(HttpStatusCode.NotFound, $"Product ID [{id}] not found.");

            return Ok(brands);
        }
        /// <summary>
        ///   Gets the Brands by PRoduct Code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByProductCode/{code}/{isActive}/{customerId}")]
        public IHttpActionResult GetByProductCode(string code, bool isActive, long customerId)
        {
            var brands = _brandService.GetByProductCode(code, isActive, customerId);
            if (brands == null)
                return Content(HttpStatusCode.NotFound, $"Product Code [{code}] not found.");

            return Ok(brands);
        }

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            var brands = _brandService.GetAll();
            return Ok(brands);
        }

        [HttpGet]
        [Route("GetSelectList")]
        public IHttpActionResult GetSelectList()
        {
            var brands = _brandService.GetAll();
             if (brands == null)
                return Content(HttpStatusCode.NotFound, $"Brand List empty");

            return Ok(brands);
        }

        [HttpGet]
        [Route("GetSelectListByItemCode/{code}")]
        public IHttpActionResult GetSelectListByItemCode(string code)
        {
            var brands = _brandService.GetByItemCode(code);
            if (brands == null)
                return Content(HttpStatusCode.NotFound, $"Item Code [{code}] not found.");

            return Ok(brands);
        }

        /// <summary>
        /// Adds the location record.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Post(Brand brand)
        {
            long retId;
            try
            {
                if (brand == null) throw new ArgumentException("Brand model is null.");
                retId = _brandService.Add(brand);
             
                if (retId == 0)
                {
                    Log.Info($"{typeof(BrandController).FullName}||{UserEnvironment}||Add record not successful, Brand Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, String.Format(Resource.Strings.m_IsDuplicate, Resource.Strings.s_BrandCode));
                }
                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Brand added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                Log.Info($"{typeof(BrandController).FullName}||{UserEnvironment}||Add record successful.");
                return ResponseMessage(response);
    
            }
            catch (Exception e)
            {
                Log.Error(typeof(BrandController).FullName, e);

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
        public IHttpActionResult Put(Brand obj)
        {

            bool isNotDuplicate = _brandService.Update(obj);
            if (isNotDuplicate == true)
            {
                Log.Info($"{typeof(BrandController).FullName}||{UserEnvironment}||"+String.Format(Resource.String.m_RecordSuccessful,Resource.String.s_Update));
                return Content(HttpStatusCode.OK, String.Format(Resource.String.m_RecordWasUpdatedSuccessfully, Resource.String.s_Brand));
            }

            Log.Info($"{typeof(BrandController).FullName}||{UserEnvironment}||Update record not successful, Brand Code is duplicate.");
            return Content(HttpStatusCode.Forbidden, String.Format(Resource.Strings.m_IsDuplicate, Resource.Strings.s_BrandCode));

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
                _brandService.Delete(id, updatedBy);
                Log.Info($"{typeof(BrandController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Brand record was deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Enables the record
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
                _brandService.Enable(id, updatedBy);
                Log.Info($"{typeof(BrandController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Brand record was enabled.");
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
                var obj = _brandService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _brandService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }

    }
}
