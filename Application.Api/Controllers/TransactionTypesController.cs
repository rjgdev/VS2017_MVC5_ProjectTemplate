using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Newtonsoft.Json;

namespace Application.Api.Controllers
{
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/TransactionTypes")]
    public class TransactionTypesController : BaseApiController
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionTypesController(ITransactionTypeService transactionType)
        {
            _transactionTypeService = transactionType;
        }

        //[HttpGet]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var vendor = _transactionTypeService.GetList(take);
        //    if (vendor == null)
        //        return Content(HttpStatusCode.NotFound, $"No vendor record were found.");

        //    return Ok(vendor);
        //}

        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var vendor = _transactionTypeService.GetById(id);
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"Vendor ID [{id}] not found.");

            return Ok(vendor);
        }


        [HttpPost]
        [Route("Add")]
        // POST api/<controller>
        public IHttpActionResult Post(object obj)
        {
            long retId;
            try
            {
                retId = _transactionTypeService.Add(obj);

                if (retId == 0)
                {
                    Log.Info($"{typeof(TransactionTypesController).FullName}||{UserEnvironment}||Add record not successful, Transaction Type Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Transaction Type Code is Duplicate");
                }
                Log.Info($"{typeof(TransactionTypesController).FullName}||{UserEnvironment}||Add record successful.");
            }
            catch (Exception e)
            {
                Log.Error(typeof(TransactionTypesController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created);
            var test = JsonConvert.SerializeObject(new
            {
                id = retId,
                message = "Transaction Type added"
            });
            response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
            return ResponseMessage(response);
        }

        [HttpPut]
        [Route("Update")]
        // PUT api/<controller>/5
        public IHttpActionResult Put(object obj)
        {

            bool IsNotDuplicate = _transactionTypeService.Update(obj);
            if (IsNotDuplicate == true)
            {

                Log.Info($"{typeof(TransactionTypesController).FullName}||{UserEnvironment}||Update record successful.");

                return Content(HttpStatusCode.OK, "Transaction type record was updated successfully.");
            }
            Log.Info($"{typeof(TransactionTypesController).FullName}||{UserEnvironment}||Update record not successful, Transaction type is duplicate.");
            return Content(HttpStatusCode.Forbidden, "Transaction type Code is Duplicate");
        }

        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _transactionTypeService.Delete(id,updatedBy);
                Log.Info(
                    $"{typeof(TransactionTypesController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "Transaction type record deleted");
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
                _transactionTypeService.Enable(id,updatedBy);
                Log.Info(
                    $"{typeof(TransactionTypesController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Transaction type record enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetSelectList")]
        public IHttpActionResult GetSelectList()
        {
            var transTypes = _transactionTypeService.GetAll();
            if (transTypes == null)
                return Content(HttpStatusCode.NotFound, $"No status record were found.");

            return Ok(transTypes);
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
                var obj = _transactionTypeService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _transactionTypeService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
           

        }
    }
}













