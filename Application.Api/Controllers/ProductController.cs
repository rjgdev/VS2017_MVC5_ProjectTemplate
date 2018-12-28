using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Application.Api.Filters;
using Application.Bll;
using Application.Model;
using Newtonsoft.Json;
using System.Text;

namespace Application.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Product")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region GET
        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var customer = _productService.GetById(id);
            if (customer == null)
                return Content(HttpStatusCode.NotFound, $"Customer Id [{id}] not found.");

            return Ok(customer);
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetByDomain/{domain}")]
        public IHttpActionResult GetByProductCode(string productCode)
        {
            var product = _productService.GetByProductCode(productCode);
            if (product == null)
                return Content(HttpStatusCode.NotFound, $"Product domain [{productCode}] not found.");

            return Ok(product);
        }

        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var customer = _productService.GetList(take);
        //    if (customer == null)
        //        return Content(HttpStatusCode.NotFound, $"Product list empty");

        //    return Ok(customer);
        //}

        #endregion GET

        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        public IHttpActionResult Add(Product model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                long retId = _productService.Add(model);

                bool IsDuplicate = retId == 0;
                if (IsDuplicate == true)
                {
                    Log.Info($"{typeof(ProductController).FullName}||{UserEnvironment}||Add record not successful, Location Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Location Code is Duplicate");
                }

                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Product added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                Log.Info($"{typeof(ProductController).FullName}||{UserEnvironment}||Add record successful.");
                return ResponseMessage(response);
                
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [RequireHttps]
        [Route("Update")]
        public IHttpActionResult Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool IsNotDuplicate = _productService.Update(product);
                if (IsNotDuplicate == true)
                {
                    Log.Info($"{typeof(ProductController).FullName}||{UserEnvironment}||Update record successful.");
                    return Content(HttpStatusCode.OK, "Product updated successfully");

                }

                Log.Info($"{typeof(ProductController).FullName}||{UserEnvironment}||Update record not successful, Product Code is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Product Code is Duplicate");
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [RequireHttps]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _productService.Delete(id,updatedBy);
                Log.Info($"{typeof(ProductController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Item deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [RequireHttps]
        [Route("Enable/{id}/{updatedBy}")]
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _productService.Enable(id,updatedBy);
                Log.Info($"{typeof(ProductController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Item enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetSelectList")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetSelectList()
        {
            var selectList = _productService.GetSelectList();
            if (selectList == null)
                return Content(HttpStatusCode.NotFound, $"Product list empty");

            return Ok(selectList);
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
                var obj = _productService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _productService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
    }
}
