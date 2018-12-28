using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Http;
using Application.Api.Filters;
using Newtonsoft.Json;
using Application.Bll;
using Application.Model;
using Application.Bll.Models;
using Newtonsoft.Json.Linq;

namespace Application.Api.Controllers
{
    /// <summary>
    ///     Delivery Request API
    /// </summary>
    [Authorize]
    [RoutePrefix("api/GoodsOut/DeliveryRequest")]
    public class DeliveryRequestController : BaseApiController
    {
        private readonly IDeliveryRequestService _deliveryRequestService;
        private readonly IDeliveryRequestLineService _deliveryRequestLineService;
        private readonly IDeliveryRequestLineItemService _deliveryRequestLineItemService;
        private readonly IWarehouseService _warehouseService;

        public DeliveryRequestController(IDeliveryRequestService deliveryRequestService,
            IDeliveryRequestLineItemService deliveryRequestLineItemService, IDeliveryRequestLineService deliveryRequestLineService,
            IWarehouseService warehouseService)
        {
            _deliveryRequestService = deliveryRequestService;
            _deliveryRequestLineService = deliveryRequestLineService;
            _deliveryRequestLineItemService = deliveryRequestLineItemService;
            _warehouseService = warehouseService;
        }

        #region Lines

        /// <summary>
        ///     Add delivery request line record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Line/Add")]
        // POST api/<controller>
        public IHttpActionResult AddLine(object obj)
        {
            try
            {
                var objModel = JObject.Parse(obj.ToString());
                var deliveryRequestLines = (dynamic)objModel;
                if (deliveryRequestLines.Quantity < 1)
                {
                    return Content(HttpStatusCode.Forbidden, "Quantity is must be greater than 0");
                }
                var retId = _deliveryRequestService.AddDeliveryRequestLine(obj);
                if (retId == 0)
                {
                    Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||AddLine||Add Delivery Request Line not successful. Item is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Item is Duplicate");
                }

                return Content(HttpStatusCode.Created, new
                {
                    id = retId,
                    message = "Success"
                });
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }


            //{
            //    "DeliveryRequestId": 1,
            //    "LineNumber":1,
            //    "ProductId": 1,
            //    "PickType":"FIFO",
            //    "Quantity":2,
            //    "SpecialInstructions":"",
            //    "Memo":""
            //}
        }

        /// <summary>
        /// Add delivery request line record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Line/Add/Batch")]
        // POST api/<controller>
        public IHttpActionResult AddLines(List<object> obj)
        {
            try
            {
                var retId = _deliveryRequestService.AddDeliveryRequestLine(obj);

                return Content(HttpStatusCode.Created, new
                {
                    message = "Success"
                });
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        /// <summary>
        /// Update delivery request line record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Line/Update")]
        // PUT api/<controller>/5
        public IHttpActionResult UpdateLine(object obj)
        {
            try
            {
                bool IsNotDuplicate = _deliveryRequestService.UpdateDeliveryRequestLine(obj);
                if (IsNotDuplicate == true)
                {
                    Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||UpdateLine||Update record successful.");
                    return Content(HttpStatusCode.OK, "Delivery request line record was updated successfully.");
                }

                Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||Update record not successful, Item is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Item is duplicate.");
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        /// <summary>
        /// Update delivery request line record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Line/BatchUpdate")]
        // PUT api/<controller>/5
        public IHttpActionResult UpdateLines(object obj)
        {
            try
            {
                bool IsNotDuplicate = _deliveryRequestService.UpdateDeliveryRequestLines(obj);
                if (IsNotDuplicate == true)
                {
                    Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||UpdateLine||Update record successful.");
                    return Content(HttpStatusCode.OK, "Delivery request line record was updated successfully.");
                }

                Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||Update record not successful, some items are duplicate.");
                return Content(HttpStatusCode.Forbidden, "Some items are duplicate.");
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }



        [HttpPut]
        [RequireHttps]
        [Route("Line/Update/Batch")]
        // PUT api/<controller>/5
        public IHttpActionResult MobileUpdateLines(List<DeliveryRequestLine> objs)
        {
            try
            {
                if(objs.Count() > 0)
                {
                    bool ret = _deliveryRequestService.BatchUpdateDeliveryRequestLine(objs);
                    if (ret == true)
                    {
                        Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||UpdateLine||Update record successful.");
                        return Content(HttpStatusCode.OK, "Delivery request line record was updated successfully.");
                    }
                    else return Content(HttpStatusCode.BadRequest, "Delivery request line record was not updated successfully.");
                }
                else return Content(HttpStatusCode.OK, "No lines to be updated");

            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

           
        
        }

        /// <summary>
        ///     Delete delivery request line record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RequireHttps]
        [Route("Line/Delete/{id}/{updatedBy}")]
        public IHttpActionResult DeleteLine(long id, string updatedBy)
        {
            try
            {
                _deliveryRequestService.DeleteDeliveryRequestLine(id, updatedBy);
                return Content(HttpStatusCode.NoContent, "Delivery request line record deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///     Enable delivery request line record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Line/Enable/{id}/{updatedBy}")]
        public IHttpActionResult EnableLine(int id, string updatedBy)
        {
            try
            {
                _deliveryRequestService.EnableDeliveryRequestLine(id, updatedBy);
                return Content(HttpStatusCode.NoContent, "Delivery request line record enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get delivery request lines
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("Line/GetList/{take}")]
        //public IHttpActionResult GetLineList(int take)
        //{
        //    var vendor = _deliveryRequestService.GetDeliveryRequestLineList(take);
        //    if (vendor == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request line record were found.");

        //    return Ok(vendor);
        //}

        /// <summary>
        /// Get delivery request line by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/GetById/{id}")]
        public IHttpActionResult GetLineById(long id)
        {
            var obj = _deliveryRequestService.GetDeliveryRequestLineByIdDynamic(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// Get delivery request line by delivery request ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/GetLineByDeliveryRequestId/{id}")]
        public IHttpActionResult GetLineByDeliveryRequestId(long id)
        {
            var obj = _deliveryRequestService.GetLinesByDeliveryRequestId(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// Get delivery request line by delivery request ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/Dynamic/GetLineByDeliveryRequestId/{id}")]
        public IHttpActionResult GetLineDynamicByDeliveryRequestId(long id)
        {
            var obj = _deliveryRequestService.GetLinesByDeliveryRequestIdDynamic(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <param name="deliveryRequestId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/GetList/{isActive}/{customerId}/{deliveryRequestId}/{pageNo?}/{pageSize?}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetLineList(bool isActive, long customerId, long deliveryRequestId,
            int? pageNo = null, int? pageSize = null)
        {

            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _deliveryRequestLineService.GetListDynamic(isActive, customerId, deliveryRequestId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _deliveryRequestLineService.GetListDynamic(isActive, customerId, deliveryRequestId,
                    (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }
        
        #endregion

        #region Line Items

        /// <summary>
        ///     Add delivery request line item record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Line/Item/Add")]
        // POST api/<controller>
        public IHttpActionResult AddLineItem(object obj)
        {
            try
            {
                var retId = _deliveryRequestService.AddLineItem(obj);
                return Content(HttpStatusCode.Created, new
                {
                    id = retId,
                    message = "Success"
                });
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

            //{
            //    "DeliverRequestLineItemId":4,
            //    "ItemId":1
            //}
        }

        [HttpPut]
        [RequireHttps]
        [Route("Line/Item/Update")]
        // PUT api/<controller>/5
        public IHttpActionResult UpdateLineItem(object obj)
        {
            try
            {
                _deliveryRequestService.UpdateLineItem(obj);
                return Content(HttpStatusCode.NoContent, "Delivery request line item record was updated successfully.");
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [HttpPut]
        [RequireHttps]
        [Route("Line/Item/BatchUpdate")]
        // PUT api/<controller>/5
        public IHttpActionResult UpdateLineItem(List<object> obj)
        {
            try
            {
                _deliveryRequestService.UpdateLineItem(obj);
                return Content(HttpStatusCode.NoContent, "Delivery request line item record was updated successfully.");
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        /// <summary>
        ///     Delete delivery request line item record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RequireHttps]
        [Route("Line/Item/Delete/{id}")]
        public IHttpActionResult DeleteLineItem(int id)
        {
            try
            {
                _deliveryRequestService.DeleteLineItem(id);
                return Content(HttpStatusCode.NoContent, "Delivery request line item record deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get Line Items
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("Line/Item/GetList/{take}")]
        //public IHttpActionResult GetLineItemList(int take)
        //{
        //    var vendor = _deliveryRequestService.GetLineItemList(take);
        //    if (vendor == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request line item record were found.");

        //    return Ok(vendor);
        //}

        /// <summary>
        /// Get Line Item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/Item/GetById/{id}")]
        public IHttpActionResult GetLineItemById(int id)
        {
            var obj = _deliveryRequestService.GetLineItemById(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line item record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// Get the list of line items by Line ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/Item/GetListByLineId/{id}")]
        public IHttpActionResult GetLineItemsByLineId(long id)
        {
            var obj = _deliveryRequestService.GetLineItemsByLineId(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line item record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// Get the list of line items by Line ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/Item/Dynamic/GetListByLineId/{id}")]
        public IHttpActionResult GetLineItemsDynamicByLineId(long id)
        {
            var obj = _deliveryRequestService.GetLineItemsDynamicByLineId(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line item record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// Get Line Items by Delivery Request ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/Item/GetListByDeliveryRequestId/{id}")]
        public IHttpActionResult GetLineItemsByDeliveryRequestId(long id)
        {
            var obj = _deliveryRequestLineItemService.GetLineItemsByDeliveryRequestId(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line item record were found.");

            return Ok(obj);
        }

        /// <summary>
        /// Get Line Items by Delivery Request Code
        /// </summary>
        /// <param name="requestCode"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("Line/Item/GetListByDeliveryRequestCode/{requestcode}")]
        public IHttpActionResult GetLineItemsByDeliveryRequestCode(string requestCode)
        {
            var obj = _deliveryRequestLineItemService.GetLineItemsByDeliveryRequestCode(requestCode);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line item record were found.");

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
        [Route("GetLineItemList/{isActive}/{CustomerId}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetLineItemList(bool isActive, long customerId, int? pageNo, int? pageSize)
        {

            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _deliveryRequestLineItemService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                return Content(HttpStatusCode.NotImplemented, $"Pagination not Implemented");
            }


        }
        #endregion

        #region Header

        /// <summary>
        ///     Gets the delivery request listing
        /// </summary>
        /// <param name="take"></param>
        /// <returns>List</returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var obj = _deliveryRequestService.GetList(take);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

        //    Log.Info(string.Format(
        //        typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetList()||Count::{1}",
        //        UserEnvironment, obj.Count()));

        //    return Ok(obj);
        //}

        /// <summary>
        ///     Gets the delivery request listing
        /// </summary>
        /// <param name="take"></param>
        /// <returns>List</returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("Dynamic/GetList/{take}")]
        //public IHttpActionResult GetDynamicList(int take)
        //{
        //    var obj = _deliveryRequestService.GetDynamicList(take);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

        //    Log.Info(string.Format(
        //        typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetList()||Count::{1}",
        //        UserEnvironment, obj.Count()));

        //    return Ok(obj);
        //}

        /// <summary>
        ///     Get list by pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{page}/{pageSize}")]
        //public IHttpActionResult GetList(int page = 0, int pageSize = 10)
        //{
        //    var obj = _deliveryRequestService.GetList(page, pageSize);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

        //    Log.Info(string.Format(
        //        typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetList()||Count::{1}",
        //        UserEnvironment, obj.Count()));

        //    return Ok(obj);
        //}

        /// <summary>
        /// Get Delivery Request List
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>IEnumerable dynamic object.</returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("GetDynamicList/{page}/{pageSize}")]
        //public IHttpActionResult GetDynamicList(int page = 0, int pageSize = 10)
        //{
        //    var obj = _deliveryRequestService.GetDynamicList(page, pageSize);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

        //    Log.Info(string.Format(
        //        typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetDynamicList()||Count::{1}",
        //        UserEnvironment, obj.Count()));

        //    return Ok(obj);
        //}

        /// <summary>
        /// Get Delivery Request List by Allocated Status 
        /// </summary>
        /// <returns>List</returns>
        //[HttpGet]
        //[RequireHttps]
        //[Route("GetByAllocated")]
        //public IHttpActionResult GetByAllocated(bool isActive, long customerId)
        //{
        //    var status = "Allocated";
        //    var obj = _deliveryRequestService.GetByStatus(status);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

        //    Log.Info(string.Format(
        //        typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetList()||Count::{1}",
        //        UserEnvironment, obj.Count()));

        //    return Ok(obj);

        //}

        /// <summary>
        /// Get Delivery Request List by Allocated Status 
        /// </summary>
        /// <returns>List</returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetByAllocated/{isActive}/{customerId}")]
        public IHttpActionResult GetByAllocated(bool isActive, long customerId)
        {
            // Added is processing parameter in service 
            var statusId = 13;
            var obj = _deliveryRequestService.GetByStatusIdDynamic(isActive, customerId, statusId);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

            Log.Info(string.Format(
                typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetList()||Count::{1}",
                UserEnvironment, obj.Count()));

            return Ok(obj);

        }

        /// <summary>
        /// Get Delivery Request List by Status 
        /// </summary>
        /// <param name="status"></param>
        /// <returns>List</returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetByStatus/{status}/{isActive}/{customerId}")]
        public IHttpActionResult GetByStatus(string status, bool isActive, long customerId, int? pageSize = null, int? pageNo = null)
        {
            var obj = _deliveryRequestService.GetByStatusDynamic(isActive, customerId, status);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

            Log.Info(string.Format(
                typeof(DeliveryRequestController).Name + "||User Environment::{0}||GetList()||Count::{1}",
                UserEnvironment, obj.Count()));

            return Ok(obj);

        }


        /// <summary>
        /// Get delivery request by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var obj = _deliveryRequestService.GetByIdDynamic(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request record were found.");

            return Ok(obj);
        }

        /// <summary>
        ///     Add delivery request record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        // POST api/<controller>
        //public IHttpActionResult AddDeliveryRequest(object obj)
        public IHttpActionResult AddDeliveryRequest(object obj)
        {
            try
            {
                string deliveryRequestCode = "";
                var retId = _deliveryRequestService.AddDeliveryRequest(obj, out deliveryRequestCode);

                if (retId == 0)
                {
                    Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||Add Delivery Request not successful, Delivery Request Code is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Delivery Request Code is Duplicate");
                }
                
                Log.Info(
                    $"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||AddDeliveryRequest()||Add record successful.");

                return Content(HttpStatusCode.Created,
                    new {
                        id = retId,
                        deliveryRequestCode = deliveryRequestCode,
                        message = "Success"
                    });
            }
            catch (DbEntityValidationException ex)
            {
                dynamic errlist = new
                {
                    Message = "Validation error",
                    ErrorList = (from err in ex.EntityValidationErrors
                        from error in err.ValidationErrors
                        select error.ErrorMessage).Cast<object>().ToList()
                };

                Log.Error(typeof(DeliveryRequestController).FullName, ex);

                return Content(HttpStatusCode.BadRequest, errlist);
            }
            catch (Exception e)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

            //{
            //    "DeliveryRequestCode":"A101",
            //    "RequestType":"Customer Despatch",
            //    "RequestedDate":"",
            //    "RequiredDeliveryDate":"",
            //    "HaulierId":1,
            //    "ServiceCode":"",
            //    "CustomerRef":"",
            //    "RequiredDate":"",
            //    "EarliestDate":"",
            //    "LatestDate":"",
            //    "SalesOrderRef":"",
            //    "WarehouseId":1,
            //    "Priority":50,
            //    "IsFullfilled":false,
            //    "CustomerClientId":1,
            //    "CustomerId":1
            //}
        }

        /// <summary>
        ///     Update delivery request record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Update")]
        public IHttpActionResult UpdateDeliveryRequest(object obj)
        {
            try
            {
               bool IsNotDuplicate = _deliveryRequestService.UpdateDeliveryRequest(obj);
                if (IsNotDuplicate)
                {
                    Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||UpdateDeliveryRequest()||Update record successful.");
                    return Content(HttpStatusCode.OK, "Delivery request record was updated successfully.");
                }
                Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||Update record not successful, Delivery Request Code is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Delivery Request Code is Duplicate");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(DeliveryRequestController).FullName, ex);

                return Content(HttpStatusCode.NotAcceptable, ex.Message);
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("Despatched")]
        public IHttpActionResult DispatchDeliveryRequest(DeliveryRequestBindingModel obj)
        {


            try
            {
                _deliveryRequestService.DispatchDeliveryRequest(obj);
                Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}|| Despatched successful.");

                return Content(HttpStatusCode.OK, "Despatched successful ");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



        /// <summary>
        ///     Delete delivery request record.
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
                _deliveryRequestService.Delete(id,updatedBy);
                Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "Delivery request record deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///     Delete delivery request record.
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
                _deliveryRequestService.Enable(id,updatedBy);
                Log.Info($"{typeof(DeliveryRequestController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Delivery request record enabled");
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
        /// <param name="statusId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetList/{isActive}/{customerId}/{statusId}/{pageNo?}/{pageSize?}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetList(bool isActive, long customerId, long statusId, int? pageNo = null, int? pageSize = null)
        {
            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _deliveryRequestService.GetListDynamic(isActive, customerId, statusId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _deliveryRequestService.GetListDynamic(isActive, customerId, statusId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }


        [HttpGet]
        [RequireHttps]
        [Route("GetAvailableList/{isActive}/{customerId}/")]
        // GetList api/<controller>/5
        public IHttpActionResult GetAvailableList(bool isActive, long customerId)
        {

            var obj = _deliveryRequestService.GetAvailableListDynamic(isActive, customerId,false);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No data found");
            return Ok(obj);


        }



        [HttpGet]
        [RequireHttps]
        [Route("CheckIsProcessing/{id}")]
        // GetList api/<controller>/5
        public IHttpActionResult CheckIsProcessing(long id)
        {

            if(id != 0)
            {
                var retval = _deliveryRequestService.CheckIsProcessing(id);
                return Content(HttpStatusCode.OK, new { IsProcessing = retval});
            }
            else return Content(HttpStatusCode.NotFound, $"No data found");


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isProcess"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        [HttpPut]
        [RequireHttps]
        [Route("OnProcessing/{id}/{isProcess}/{updatedBy}")]
        // GetList api/<controller>/
        public IHttpActionResult OnProcessing(long id,bool isProcess, string updatedBy)
        {

            if (id != 0)
            {
               var ret = _deliveryRequestService.Onprocessing(id,isProcess,  updatedBy);
                if (ret == true)  return Content(HttpStatusCode.OK, $"Successfully Assigned");
                else return Content(HttpStatusCode.Forbidden, $"Not Assigned Successfully");
            }
            else
            {
                return Content(HttpStatusCode.NotFound, $"No data found");
            }

        }

        #endregion


    }

   
}