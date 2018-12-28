using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Application.Bll.Models;
using Application.Model;
using Newtonsoft.Json;

namespace Application.Api.Controllers
{
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/Warehouse")]
    public class WarehouseController : BaseApiController
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult Get(int id)
        {
            var warehouse = _warehouseService.GetById(id);
            if (warehouse == null)
                return Content(HttpStatusCode.NotFound, $"Warehouse ID [{id}] not found.");

            //var warehouseModel = new WarehouseBindingModel
            //{
            //    //AddressCode = warehouse.AddressCode,
            //    //Domain = warehouse.Customer.Domain,
            //    WarehouseCode = warehouse.WarehouseCode,
            //    Id = warehouse.Id,
            //    Description = warehouse.Description
            //};

            return Ok(warehouse);
        }

        //[HttpGet]
        //[Route("GetById/{warehouseCode}")]
        //public IHttpActionResult Get(string wareshouseCode)
        //{
        //    var warehouse = _warehouseService.GetByWarehouseCode(wareshouseCode);
        //    if (warehouse == null)
        //        return Content(HttpStatusCode.NotFound, $"WareshouseCode [{wareshouseCode}] not found.");

        //    var warehouseModel = new WarehouseBindingModel
        //    {
        //        //AddressCode = warehouse.AddressCode,
        //        //Domain = warehouse.Customer.Domain,
        //        WarehouseCode = warehouse.WarehouseCode,
        //        Id = warehouse.Id,
        //        Description = warehouse.Description
        //    };
        //    return Ok(warehouseModel);
        //}

        //[HttpGet]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var warehouses = _warehouseService.GetList(take);
        //    if (warehouses == null)
        //        return Content(HttpStatusCode.NotFound, $"Wareshouse List empty");

        //    var retval = warehouses.Select(warehouse => new WarehouseBindingModel
        //    {
        //        //AddressCode = warehouse.AddressCode,
        //        //Domain = warehouse.Customer.Domain,
        //        WarehouseCode = warehouse.WarehouseCode,
        //        Id = warehouse.Id,
        //        Description = warehouse.Description
        //    });
        //    return Ok(retval);
        //}

        [HttpGet]
        [Route("GetSelectList")]
        public IHttpActionResult GetSelectList()
        {
            var warehouses = _warehouseService.GetAll();
            if (warehouses == null)
                return Content(HttpStatusCode.NotFound, $"Wareshouse List empty");

            return Ok(warehouses);
        }


        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(Warehouse warehouseModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retId = _warehouseService.Add(warehouseModel);
                bool IsDuplicate = retId == 0;

                if (IsDuplicate == true)
                {
                    Log.Info($"{typeof(WarehouseController).FullName}||{UserEnvironment}||Add record not successful, Warehouse Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Warehouse Code is Duplicate");
                }

                Log.Info($"{typeof(WarehouseController).FullName}||{UserEnvironment}||Add record successful.");

                var response = Request.CreateResponse(HttpStatusCode.Created);
                var test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Warehouse added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(Warehouse warehouseModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                bool IsNotDuplicate = _warehouseService.Update(warehouseModel);
                if (IsNotDuplicate == true)
                {

                    Log.Info($"{typeof(WarehouseController).FullName}||{UserEnvironment}||Update record successful.");
                    return Content(HttpStatusCode.OK, "Warehouse added successfully");
                }
                Log.Info($"{typeof(WarehouseController).FullName}||{UserEnvironment}||Update record not successful, Warehouse Code is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Warehouse Code is Duplicate");
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
                _warehouseService.Delete(id,updatedBy);
                Log.Info($"{typeof(WarehouseController).FullName}||{UserEnvironment}||Delete record successful.");
            }
            catch (Exception)
            {
                throw;
            }
            return Content(HttpStatusCode.OK, "Warehouse deleted successfully");
        }

        [HttpPut]
        [Route("Enable/{id}/{updatedBy}")]
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _warehouseService.Enable(id,updatedBy);
                Log.Info($"{typeof(WarehouseController).FullName}||{UserEnvironment}||Enable record successful.");
            }
            catch (Exception)
            {
                throw;
            }
            return Content(HttpStatusCode.OK, "Warehouse enabled successfully");
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
                var obj = _warehouseService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _warehouseService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}