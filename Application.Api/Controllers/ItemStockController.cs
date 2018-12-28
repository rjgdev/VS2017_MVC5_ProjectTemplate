using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Application.Bll.Models;
using Application.Model;

namespace Application.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/ItemStock")]
    public class ItemStockController : BaseApiController
    {
        private readonly IItemStockService _itemStockService;
        private readonly IItemService _itemService;
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;
        private readonly ILocationService _locationService;
        private readonly IBrandService _brandService;

        public ItemStockController(IItemStockService itemStockService, IItemService itemService, IProductService productService, IWarehouseService warehouseService,
          ILocationService locationService, IBrandService brandService)
        {
            _itemStockService = itemStockService;
            _productService = productService;
            _warehouseService = warehouseService;
            _locationService = locationService;
            _brandService = brandService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult Get(int id)
        {
            var obj = _itemStockService.GetById(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"Item ID [{id}] not found.");
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
        public IHttpActionResult GetList(bool isActive, long customerId, int? pageNo = null, int? pageSize = null)
        {

            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _itemStockService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _itemStockService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        public IHttpActionResult Add(ItemStock obj)
        {
            var retId = _itemStockService.Add(obj);
            if (retId == 0)
                return Content(HttpStatusCode.NotFound, $"Item ID [{obj}] not found.");
            return Ok(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Update")]
        public IHttpActionResult Update(ItemStock obj)
        {
            var retId = _itemStockService.Update(obj);
            if (retId != true)
                return Content(HttpStatusCode.NotFound, $"Item ID [{obj}] not found.");
            return Ok(obj);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Enable/{id}/{updatedBy}")]
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _itemStockService.Enable(id,updatedBy);
                Log.Info($"{typeof(ItemStockController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Item enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [RequireHttps]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _itemStockService.Delete(id,updatedBy);
                Log.Info($"{typeof(ItemStockController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Item enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



    }

   
}
