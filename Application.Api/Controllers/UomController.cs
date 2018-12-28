using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Application.Api.Filters;
using Application.Bll;
using Application.Model;
using Newtonsoft.Json;

namespace Application.Api.Controllers
{
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/Uom")]
    public class UomController : BaseApiController
    {
        private readonly IUomService _uomService;


        public UomController(IUomService uomService)
        {
            _uomService = uomService;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var uom = _uomService.GetById(id);
            if (uom == null)
                return Content(HttpStatusCode.NotFound, $"UoM Id [{id}] not found.");

            return Ok(uom);
        }

        /// <summary>
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("Getlist/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var uom = _uomService.GetList(take);
        //    if (uom == null)
        //        return Content(HttpStatusCode.NotFound, $"UoM List empty.");

        //    return Ok(uom);
        //}

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(Uom model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var retId = _uomService.Add(model);
                
                if (retId == 0)
                {
                    Log.Info($"{typeof(UomController).FullName}||{UserEnvironment}||Add record not successful, Uom Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Uom Code is Duplicate");
                    
                }
                else
                {

                    var response = Request.CreateResponse(HttpStatusCode.Created);
                    var test = JsonConvert.SerializeObject(new
                    {
                        id = retId,
                        message = "Unit of measure added"
                    });
                    Log.Info($"{typeof(UomController).FullName}||{UserEnvironment}||Add record successful.");
                    response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                    return ResponseMessage(response);
                }
                
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(Uom uom)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var IsNotDuplicate = _uomService.Update(uom);
                if (IsNotDuplicate == true)
                {
                     Log.Info($"{typeof(UomController).FullName}||{UserEnvironment}||Update record successful.");
                    return Content(HttpStatusCode.OK, "Uom updated successfully");
                }
                else
                {
                    Log.Info($"{typeof(UomController).FullName}||{UserEnvironment}||Update record not successful, Uom Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Uom Code is Duplicate");
                }
                
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _uomService.Delete(id,updatedBy);
                Log.Info($"{typeof(UomController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "Uom deleted");
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
                _uomService.Enable(id,updatedBy);
                Log.Info($"{typeof(UomController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Uom enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetSelectList")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetSelectList()
        {
            var uom = _uomService.GetSelectList();
            if (uom == null)
                return Content(HttpStatusCode.NotFound, $"UoM List empty.");

            return Ok(uom);
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
                var obj = _uomService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _uomService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}