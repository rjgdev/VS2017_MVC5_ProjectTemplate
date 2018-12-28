using System.Net;
using System.Web.Http;
using Application.Bll;
using Application.Bll.Models;
using System;
using System.Web.Http.Description;
using Application.Api.Filters;
using Application.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Application.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Item")]
    public class ItemController : BaseApiController
    {
        private readonly IItemService _itemService;
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;
        private readonly ILocationService _locationService;
        private readonly IBrandService _brandService;
        private readonly IUomService _uomService;

        public ItemController(IItemService itemService, IProductService productService, IWarehouseService warehouseService,
            ILocationService locationService, IBrandService brandService, IUomService uomService)
        {
            _itemService = itemService;
            _productService = productService;
            _warehouseService = warehouseService;
            _locationService = locationService;
            _brandService = brandService;
            _uomService = uomService;
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult Get(int id)
        {
            var item = _itemService.GetById(id);
            if (item == null)
                return Content(HttpStatusCode.NotFound, $"Item ID [{id}] not found.");
            return Ok(item);
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetByItemCode/{itemCode}/{customerId}")]
        public IHttpActionResult Get(string itemCode,long? customerId)
        {
            var item = _itemService.GetByItemCode(itemCode, customerId);
            if (item == null)
                return Content(HttpStatusCode.NotFound, $"Item code [{itemCode}] not found.");

            var product = _productService.GetById((int)item.ProductId);

            var retVal = new ItemBindingModel
            {
                ProductCode = product.ProductCode,
                Description = item.Description,
                ItemCode = item.ItemCode
            };


            return Ok(retVal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var product = _itemService.GetList(take);
        //    if (product == null)
        //        return Content(HttpStatusCode.NotFound, $"Item List is empty.");

        //    return Ok(product);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var item = _itemService.GetAll();
            if (item == null)
                return Content(HttpStatusCode.NotFound, $"Item List is empty.");

            return Ok(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        public IHttpActionResult Add(object model)
        {
            long retId;
            try
            {
                retId = _itemService.Add(model);

                if (retId == 0)
                {
                    Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Add record not successful, Item Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Item Code is Duplicate");
                }
                Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Add record successful.");

                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Item added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Add record successful.");

                return ResponseMessage(response);

 

            }
            catch (Exception ex)
            {
                throw;

            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Update")]
        //public IHttpActionResult Update(ItemBindingModel model)
        public IHttpActionResult Update(object model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            try
            {
                bool IsNotDuplicate = _itemService.Update(model);
                if (IsNotDuplicate)
                {
                    Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Update record successful.");
                    return Content(HttpStatusCode.OK, $"Item updated");
                }
                Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Update record not successful, Item Code is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Item Code is Duplicate");


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RequireHttps]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _itemService.Delete(id,updatedBy);
                Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Item deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
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
                _itemService.Enable(id,updatedBy);
                Log.Info($"{typeof(ItemController).FullName}||{UserEnvironment}||Enable record successful.");
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
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetSelectListByBrandId/{brandId}")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetSelectListByBrandId(int brandId)
        {
            var list = _itemService.GetSelectListByBrandId(brandId);

            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetSelectListByProductId/{productId}")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetSelectListByProductId(int productId)
        {
            var list = _itemService.GetSelectListByProductId(productId);

            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetSelectListByProductCode/{productCode}")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetSelectListByProductCode(string productCode)
        {
            var list = _itemService.GetSelectListByProductCode(productCode);

            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetItemDescByItemCode/{id}")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetItemDescByItemCode(long id)
        {
            var desc = _itemService.GetItemDescriptionByItemCode(id);

            return Ok(desc);
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
                var obj = _itemService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _itemService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}