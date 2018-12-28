using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Application.Bll.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Application.Model;
using System.Linq;

namespace Application.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/GoodsIn/ExpectedReceipt")]
    public class ExpectedReceiptController : BaseApiController
    {
        private readonly IExpectedReceiptLineService _expectedReceiptLineService;
        private readonly IExpectedReceiptService _expectedReceiptService;

        public ExpectedReceiptController(IExpectedReceiptService expectedReceiptService,
            IExpectedReceiptLineService expectedReceiptLineService)
        {
            _expectedReceiptService = expectedReceiptService;
            _expectedReceiptLineService = expectedReceiptLineService;
        }

        #region Header

        /// <summary>
        /// Get Expectd receipt by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var ExpectedReceipt = _expectedReceiptService.GetById(id);
            if (ExpectedReceipt == null)
                return Content(HttpStatusCode.NotFound, $"ExpectedReceipt Id [{id}] not found.");

            return Ok(ExpectedReceipt);
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
        //    var ExpectedReceipt = _expectedReceiptService.GetList(take);
        //    if (ExpectedReceipt == null)
        //        return Content(HttpStatusCode.NotFound, $"ExpectedReceipt list empty");

        //    return Ok(ExpectedReceipt);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grn"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetByGrn/{grn}")]
        public IHttpActionResult GetByGrn(string grn)
        {
            var ExpectedReceipt = _expectedReceiptService.GetByGrn(grn);
            if (ExpectedReceipt == null)
                return Content(HttpStatusCode.NotFound, $"ExpectedReceipt list empty");

            return Ok(ExpectedReceipt);
        }

        [HttpGet]
        [RequireHttps]
        [Route("GetPlanned/{isActive}/{customerId}")]
        public IHttpActionResult GetPlanned(bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetPlannedReceipt(isActive,customerId);
            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetUnplanned/{isActive}/{customerId}")]
        public IHttpActionResult GetUnplanned(bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetUnplannedReceipt(isActive, customerId);
            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetForReceiving/{isActive}/{customerId}")]
        public IHttpActionResult GetForReceiving(bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetForReceiving(isActive,customerId);
            return Ok(list);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetForReceivingMobile/{isActive}/{customerId}")]
        public IHttpActionResult GetForReceivingMobile(bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetForReceivingMobile(isActive, customerId);
            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetReceived/{isActive}/{customerId}")]
        public IHttpActionResult GetReceived(bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetReceived(isActive,customerId);
            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetCompleted/{isActive}/{customerId}")]
        public IHttpActionResult GetCompleted(bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetCompleted(isActive,customerId);
            return Ok(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="take"></param>
        /// <param name="status"></param>
        /// <param name="customerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireHttps]
        [Route("GetReceipt/{from}/{to}/{take}/{status}/{customerId}/{isActive}")]
        public IHttpActionResult GetReceipt(DateTime from, DateTime to, int take, string status, bool isActive, long customerId)
        {
            var list = _expectedReceiptService.GetReceipt(from, to, take, status,isActive,customerId);
            return Ok(list);
        }
        

        [HttpGet]
        [RequireHttps]
        [Route("GetByRefNumber/{refNo}/{customerId}")]
        public IHttpActionResult GetByRefNo(string refNo, long customerId)
        {
            var list = _expectedReceiptService.GetByReferenceNo(refNo, customerId);
            return Ok(list);
        }

        [HttpGet]
        [RequireHttps]
        [Route("CheckRefNumber/{refNo}/{customerId}")]
        public IHttpActionResult CheckRefNumber(string refNo, long customerId)
        {
            var list = _expectedReceiptService.GetByReferenceNo(refNo, customerId );

            if(list != null)
            {
                return Content(HttpStatusCode.Forbidden, new { Message = "Duplicate Reference Number", ReferenceNumber = list.ReferenceNumber});
            }
            return Content(HttpStatusCode.NoContent, new { Message = "Reference Number is Available"});
        }

        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        public IHttpActionResult Add(ExpectedReceiptBindingModel obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            long retId;
            //if(obj.Planned == false) obj.GoodsReceivedNumber = String.Format("GRN-{0:0000000000}", (Int32)(DateTime.UtcNow.Subtract(new DateTime(2018, 9, 13))).TotalSeconds); 
            try
            {
                retId = _expectedReceiptService.Add(obj);
                if (retId == 0)
                {
                    Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Add record not successful, Reference Number is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Reference Number is Duplicate");
                }
                else
                {
                    Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Add record successful.");

                    var response = this.Request.CreateResponse(HttpStatusCode.Created);
                    string test = JsonConvert.SerializeObject(new
                    {
                        id = retId,
                        grn = _expectedReceiptService.GetById(retId).GoodsReceivedNumber,
                        message = "Expected Receipt added"
                    });
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
        [RequireHttps]
        [Route("Update")]
        public IHttpActionResult Update(ExpectedReceiptBindingModel ExpectedReceipt)
        {
            var result = _expectedReceiptService.Update(ExpectedReceipt);
            if(result == false)
            {
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Update record not successful, Reference Number is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Reference Number is Duplicate");
            }
            else
            {
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "ExpectedReceipt updated successfully");
            }

        }

        [HttpPut]
        [RequireHttps]
        [Route("Received")]
        public IHttpActionResult ReceivedExpectedReceipt(ExpectedReceiptBindingModel ExpectedReceipt)
        {
            var result = _expectedReceiptService.ReceivedExpectedReceipt(ExpectedReceipt);
            if (result == false)
            {
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Update record not successful, Reference Number is duplicate.");
                return Content(HttpStatusCode.Forbidden, "Reference Number is Duplicate");
            }
            else
            {
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "ExpectedReceipt updated successfully");
            }

        }

        [HttpDelete]
        [RequireHttps]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _expectedReceiptService.Delete(id,updatedBy);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Delete record successful.");

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
                _expectedReceiptService.Enable(id,updatedBy);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "Item enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [RequireHttps]
        [Route("GenerateRN/{customerId}")]
        public IHttpActionResult GenerateReferenceNo(long customerId)
        {
            
            var generatedReferenceNo = _expectedReceiptService.GenerateReferenceNo(customerId);
            if (customerId != 0)
                return Content(HttpStatusCode.OK, generatedReferenceNo);
            else return Content(HttpStatusCode.BadRequest, "Unknown Parameter");
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive">Bool</param>
        /// <param name="customerId">Long</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetList/{isActive}/{CustomerId}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetList(bool isActive, long customerId)
        {

            var obj = _expectedReceiptService.GetList(isActive, customerId);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No brand found");

            return Ok(obj);

        }


        [HttpGet]
        [RequireHttps]
        [Route("CheckIsProcessing/{id}")]
        // GetList api/<controller>/5
        public IHttpActionResult CheckIsProcessing(long id)
        {

            if (id != 0)
            {
                var retval = _expectedReceiptService.CheckIsProcessing(id);
                return Content(HttpStatusCode.OK, new { IsProcessing = retval });
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
        public IHttpActionResult OnProcessing(long id, bool isProcess, string updatedBy)
        {

            if (id != 0)
            {
                var ret = _expectedReceiptService.Onprocessing(id, isProcess, updatedBy);
                if (ret == true) return Content(HttpStatusCode.OK, $"Successfully Assigned");
                else return Content(HttpStatusCode.Forbidden, $"Not Assigned Successfully");
            }
            else
            {
                return Content(HttpStatusCode.NotFound, $"No data found");
            }

        }

        #endregion Header

        #region Lines

        [HttpGet]
        //[RequireHttps]
        [Route("Lines/GetList/{id}/")]
        public IHttpActionResult GetLineList(int id)
        {
            var vendor = _expectedReceiptService.GetLineList(id);
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line record were found.");

            return Ok(vendor);
        }

        [HttpGet]
        //[RequireHttps]
        [Route("Lines/GetById/{id}")]
        public IHttpActionResult GetLineById(int id)
        {
            var vendor = _expectedReceiptService.GetLineById(id);
            if (vendor == null)
                return Content(HttpStatusCode.NotFound, $"No delivery request line record were found.");

            return Ok(vendor);
        }

        [HttpPost]
        [Route("Lines/Add")]
        public IHttpActionResult AddLine(ExpectedReceiptLineBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            long retId;
            try
            {
                if(model.Quantity < 1)
                {
                    return Content(HttpStatusCode.Forbidden, "Quantity is must be greater than 0");
                }
                retId = _expectedReceiptService.AddLine(model);
                if (!string.IsNullOrEmpty(model.Image))
                    new Common().ConvertSaveImage(new
                    {
                        ImageString = model.Image,
                        FileName = String.Concat(model.GoodsReceivedNumber, "_",
                            model.ItemCode, ".jpg")
                    });

                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Add record successful.");

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
            var expectedReceiptLine = _expectedReceiptLineService.GetById(retId);
            var response = this.Request.CreateResponse(HttpStatusCode.Created);
            string test = JsonConvert.SerializeObject(new
            {
                id = retId,
                itemCode = expectedReceiptLine.ItemCode ?? "",
                message = "Expected Receipt added"
            });
            response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
            return ResponseMessage(response);
        }


        [HttpPost]
        [Route("Lines/Add/Batch")]
        public IHttpActionResult AddLine(List<ExpectedReceiptLine> model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                _expectedReceiptService.AddLine(model);

                // Will add convert image - error object reference
                //foreach (var expectedReceiptLine in model.Where(x => !string.IsNullOrEmpty(x.Image)))
                //{
                //    new Common().ConvertSaveImage(new
                //    {
                //        ImageString = expectedReceiptLine.Image,
                //        FileName = String.Concat(expectedReceiptLine.ExpectedReceipt.GoodsReceivedNumber, "_",
                //            expectedReceiptLine.ItemCode, ".jpg")
                //    });
                //}

                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Add record successful.");

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

            var response = this.Request.CreateResponse(HttpStatusCode.Created);
            string test = JsonConvert.SerializeObject(new
            {
                message = "Expected Receipt Lines added"
            });
            response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
            return ResponseMessage(response);
        }

        //[HttpPut]
        //[Route("Lines/Update")]
        //public IHttpActionResult UpdateLine(ExpectedReceiptLine model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        _expectedReceiptService.UpdateLine(model);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Content(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //    return Content(HttpStatusCode.NoContent, "ExpectedReceiptLine updated successfully");
        //}

        //[HttpPut]
        //[Route("Lines/Update")]
        //public IHttpActionResult UpdateLine(ExpectedReceiptLineBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        _expectedReceiptService.UpdateLine(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex);
        //    }
        //    return Content(HttpStatusCode.NoContent, "ExpectedReceiptLine updated successfully");
        //}

        /// <summary>
        /// Add Line Item
        /// </summary>
        /// <param name="model">Item Model</param>
        /// <param name="expectedReceiptLineId">Line ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Lines/AddLineItem/{expectedReceiptLineId}")]
        public IHttpActionResult AddLineItem(long expectedReceiptLineId,ItemBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            long retId;
            try
            {
                retId = _expectedReceiptLineService.AddLineItem(model, expectedReceiptLineId);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Add record successful.");

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

            var response = this.Request.CreateResponse(HttpStatusCode.Created);
            string test = JsonConvert.SerializeObject(new
            {
                id = retId,
                message = "Expected Receipt added"
            });
            response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
            return ResponseMessage(response);
        }

        [HttpPut]
        [Route("Lines/Update")]
        public IHttpActionResult UpdateLine(object model)
        {
            try
            {
                _expectedReceiptService.UpdateLine(model);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Update record successful.");

                return Content(HttpStatusCode.OK, "ExpectedReceiptLine updated successfully");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(ExpectedReceiptController).FullName + $"||UpdateLine()", ex);
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [Route("Lines/BatchUpdate")]
        public IHttpActionResult UpdateLines(List<dynamic> model)
        {
            try
            {
                //_expectedReceiptService.BatchUpdate(model);
                _expectedReceiptLineService.BatchUpdatExpectedReceiptLine(model);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Update record successful.");

                return Content(HttpStatusCode.OK, "ExpectedReceiptLine updated successfully");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(ExpectedReceiptController).FullName + $"||UpdateLine()", ex);
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("Lines/Delete/{id}/{updatedBy}")]
        public IHttpActionResult DeleteLine(int id, string updatedBy)
        {
            try
            {
                _expectedReceiptService.DeleteLine(id, updatedBy);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Delete record successful.");

                return Content(HttpStatusCode.OK, "ExpectedReceiptLine deleted successfully");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(ExpectedReceiptController).FullName + $"||DeleteLine()", ex);
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Lines/Enable/{id}/{updatedBy}")]
        public IHttpActionResult EnableLine(int id, string updatedBy)
        {
            try
            {
                _expectedReceiptService.EnableLine(id, updatedBy);
                Log.Info($"{typeof(ExpectedReceiptController).FullName}||{UserEnvironment}||Enable record successful.");

                return Content(HttpStatusCode.OK, "ExpectedReceiptLine enabled successfully");
            }
            catch (Exception ex)
            {
                Log.Error(typeof(ExpectedReceiptController).FullName + $"||EnableLine()", ex);
                return Content(HttpStatusCode.BadRequest, ex);
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
                var obj = _expectedReceiptLineService.GetList(isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _expectedReceiptLineService.GetList(isActive, customerId, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }

        [HttpGet]
        [Route("GetByExpectedReceipt/{expectedReceiptId}/{isActive}/{CustomerId}/{pageNo?}/{pageSize?}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetByExpectedReceipt(long expectedReceiptId,bool isActive, long customerId, int? pageNo = null, int? pageSize = null)
        {

            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _expectedReceiptLineService.GetByExpectedReceipt(expectedReceiptId, isActive, customerId);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _expectedReceiptLineService.GetByExpectedReceipt(expectedReceiptId, isActive, customerId, (int)pageNo, (int)pageSize);

                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }


        }

        [HttpGet]
        [Route("GetByExpectedReceiptMobile/{expectedReceiptId}/{isActive}/{CustomerId}/{pageNo?}/{pageSize?}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetByExpectedReceiptMobile(long expectedReceiptId, bool isActive, long customerId)
        {


            var obj = _expectedReceiptLineService.GetByExpectedReceiptMobile(expectedReceiptId, isActive, customerId);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"No data found");
            return Ok(obj);
        }


        #endregion Lines
    }
}