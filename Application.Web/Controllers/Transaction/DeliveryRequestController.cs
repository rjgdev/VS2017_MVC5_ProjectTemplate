using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using Application.Web.Helper;
using RestSharp;

namespace Application.Web.Controllers.Transaction
{
    public class DeliveryRequestController : BaseController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: DeliveryRequest
        [Authorize]
        public async Task<ActionResult> Index()
        {            
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(DeliveryRequestFilterViewModels request)
        {
            var helper = new DeliveryRequestDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<DeliveryRequestViewModel>)result.filteredList;

            var data = resultList.Select(x => new
            {
                Actions = "<div class='btn-group' style='display:flex;'>" +
                            "<a class='btn btn-primary' href='#'><i class='fa fa-gear fa-fw'></i> Action</a>" +
                            "<a class='btn btn-primary dropdown-toggle' data-toggle='dropdown' href='#'>" +
                                "<span class='fa fa-caret-down' title='Toggle dropdown menu'></span>" +
                            "</a>" +
                            "<ul class='dropdown-menu' role='menu'>" +
                                (x.IsActive && x.StatusId != 15 ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" : "") +
                                "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                                (x.IsActive && x.StatusId != 15 ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Delete</a></li>" :
                                              (x.StatusId != 15 ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>" : "")) +
                            "</ul>" +
                          "</div>",
                DeliveryRequestCode = x.IsActive ? "<a href='" + Url.Action("Index", "DeliveryRequestLine", new { x.Id }) + "'>" + x.DeliveryRequestCode + "</a>" : x.DeliveryRequestCode,
                RequestType = x.RequestType,
                RequestedDate = x.RequestedDate.ToString("MMM. dd, yyyy"),
                HaulierName = x.HaulierName,
                SalesOrderRef = x.SalesOrderRef,
                WarehouseDescription = x.WarehouseDescription,
                CustomerClientName = x.CustomerClientName,
                StatusName = x.StatusName
            });

            return Json(new
            {
                draw = request.draw,
                recordsTotal = result.totalResultCount,
                recordsFiltered = result.filteredResultCount,
                data = data
            });
        }

        // GET: DeliveryRequest/Details/5
        [Authorize]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestViewModel();
            var url = "api/GoodsOut/DeliveryRequest/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestViewModel()
                {
                    Id = data.Id,
                    IsActive = data.IsActive,
                    CustomerClientId = data.CustomerClientId,
                    CustomerClientName = data.CustomerClient,
                    CustomerId = data.CustomerId,
                    DeliveryRequestCode = data.DeliveryRequestCode,
                    CustomerRef = data.CustomerRef,
                    EarliestDate = data.EarliestDate,
                    HaulierId = data.HaulierId,
                    HaulierName = data.Haulier,
                    IsFullfilled = data.IsFullfilled,
                    LatestDate = data.LatestDate,
                    RequestedDate = data.RequestedDate,
                    RequestType = data.RequestType,
                    RequiredDate = data.RequiredDate,
                    RequiredDeliveryDate = data.RequiredDeliveryDate,
                    SalesOrderRef = data.SalesOrderRef,
                    ServiceCode = data.ServiceCode,
                    WarehouseId = data.WarehouseId,
                    WarehouseDescription = data.Warehouse,
                    Address = data.Address,
                    StatusId = data.StatusId,
                    StatusName = data.Status
                };
            }

            return PartialView(obj);
        }

        // GET: DeliveryRequest/Create
        public async Task<ActionResult> Create()
        {
            //var url = $"api/GoodsOut/DeliveryRequest/GenerateRN/{_customerId}";
            //var response = await HttpClientHelper.ApiCall(url, Method.GET, url);

            //var deliveryRequestCode = "";
            //if (response.IsSuccessful)
            //{
            //    var result = response.Content;
            //    var data = JsonConvert.DeserializeObject<dynamic>(result);

            //    deliveryRequestCode = data ?? "";
            //};

            var obj = new DeliveryRequestViewModel
            {
                CustomerId = int.Parse(CookieHelper.CustomerId),
                //DeliveryRequestCode = deliveryRequestCode,
                StatusId = 14
            };
            return PartialView(obj);
        }

        // POST: DeliveryRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DeliveryRequestCode, RequestType, RequestedDate, " +
            "RequiredDeliveryDate, HaulierId, ServiceCode, CustomerRef, RequiredDate, EarliestDate, LatestDate, " +
            "SalesOrderRef, WarehouseId, IsFullfilled, CustomerClientId, CustomerId, StatusId")] DeliveryRequestViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;
            obj.IsProcessing = false;

            var url = "api/GoodsOut/DeliveryRequest/Add";

            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(response.Content);
                var deliveryRequestCode = result.deliveryRequestCode;
                TempData["Message"] = deliveryRequestCode + " successfully created!";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Create||DeliveryRequest ID::{0}||API Response::{1}", deliveryRequestViewModel.Id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Create||DeliveryRequest ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "DeliveryRequest");
        }

        // GET: DeliveryRequest/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestViewModel();
            var url = "api/GoodsOut/DeliveryRequest/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestViewModel()
                {
                    Id = data.Id,
                    IsActive = data.IsActive,
                    CustomerClientId = data.CustomerClientId,
                    CustomerClientName = data.CustomerClient,
                    CustomerId = data.CustomerId,
                    DeliveryRequestCode = data.DeliveryRequestCode,
                    CustomerRef = data.CustomerRef,
                    EarliestDate = data.EarliestDate,
                    HaulierId = data.HaulierId,
                    HaulierName = data.Haulier,
                    IsFullfilled = data.IsFullfilled,
                    LatestDate = data.LatestDate,
                    RequestedDate = data.RequestedDate,
                    RequestType = data.RequestType,
                    RequiredDate = data.RequiredDate,
                    RequiredDeliveryDate = data.RequiredDeliveryDate,
                    SalesOrderRef = data.SalesOrderRef,
                    ServiceCode = data.ServiceCode,
                    WarehouseId = data.WarehouseId,
                    WarehouseDescription = data.Warehouse,
                    Address = data.Address,
                    StatusId = data.StatusId,
                    StatusName = data.Status,
                    CreatedBy = data.CreatedBy,
                    DateCreated = data.DateCreated,
                    IsProcessing = data.IsProcessing
                };
            }

            return PartialView(obj);
        }

        // POST: DeliveryRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, DeliveryRequestCode, RequestType, RequestedDate, RequiredDeliveryDate," +
            "HaulierId, ServiceCode, CustomerRef, RequiredDate, EarliestDate, LatestDate, SalesOrderRef, WarehouseId," +
            "IsFullfilled, CustomerClientId, CustomerId, StatusId, IsActive, CreatedBy, DateCreated, IsProcessing")] DeliveryRequestViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/GoodsOut/DeliveryRequest/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);
            if (response.IsSuccessful)
            {
                TempData["Message"] = "Delivery Request successfully updated";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Update||DeliveryRequest ID::{0}||API Response::{1}", deliveryRequestViewModel.Id, response));
                //return RedirectToAction("Index", "DeliveryRequest");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Update||DeliveryRequest ID::{0}||API Response::{1}", obj.Id, response));
                //return PartialView(deliveryRequestViewModel);
            }

            return RedirectToAction("Index", "DeliveryRequest");
        }

        // GET: DeliveryRequest/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestViewModel();
            var url = "api/GoodsOut/DeliveryRequest/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestViewModel()
                {
                    Id = data.Id,
                    CustomerClientId = data.CustomerClientId,
                    CustomerClientName = data.CustomerClient,
                    CustomerId = data.CustomerId,
                    DeliveryRequestCode = data.DeliveryRequestCode,
                    CustomerRef = data.CustomerRef,
                    EarliestDate = data.EarliestDate,
                    HaulierId = data.HaulierId,
                    HaulierName = data.Haulier,
                    IsFullfilled = data.IsFullfilled,
                    LatestDate = data.LatestDate,
                    RequestedDate = data.RequestedDate,
                    RequestType = data.RequestType,
                    RequiredDate = data.RequiredDate,
                    RequiredDeliveryDate = data.RequiredDeliveryDate,
                    SalesOrderRef = data.SalesOrderRef,
                    ServiceCode = data.ServiceCode,
                    WarehouseId = data.WarehouseId,
                    WarehouseDescription = data.Warehouse,
                    Address = data.Address,
                    StatusId = data.StatusId,
                    StatusName = data.Status
                };
            }

            return PartialView(obj);
        }

        // POST: DeliveryRequest/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var url = $"api/GoodsOut/DeliveryRequest/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Delivery Request successfully deleted!";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Delete||DeliveryRequest ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Delete||DeliveryRequest ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "DeliveryRequest");
        }
        
        // GET: DeliveryRequest/Delete/5
        [Authorize]
        public async Task<ActionResult> Enable(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestViewModel();
            var url = "api/GoodsOut/DeliveryRequest/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestViewModel()
                {
                    Id = data.Id,
                    CustomerClientId = data.CustomerClientId,
                    CustomerClientName = data.CustomerClient,
                    CustomerId = data.CustomerId,
                    DeliveryRequestCode = data.DeliveryRequestCode,
                    CustomerRef = data.CustomerRef,
                    EarliestDate = data.EarliestDate,
                    HaulierId = data.HaulierId,
                    HaulierName = data.Haulier,
                    IsFullfilled = data.IsFullfilled,
                    LatestDate = data.LatestDate,
                    RequestedDate = data.RequestedDate,
                    RequestType = data.RequestType,
                    RequiredDate = data.RequiredDate,
                    RequiredDeliveryDate = data.RequiredDeliveryDate,
                    SalesOrderRef = data.SalesOrderRef,
                    ServiceCode = data.ServiceCode,
                    WarehouseId = data.WarehouseId,
                    WarehouseDescription = data.Warehouse,
                    Address = data.Address,
                    StatusId = data.StatusId,
                    StatusName = data.Status
                };
            }

            return PartialView(obj);
        }

        // POST: DeliveryRequest/Delete/5
        [Authorize]
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {
            var url = $"api/GoodsOut/DeliveryRequest/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Delivery Request successfully enabled!";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Delete||DeliveryRequest ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestController).Name) + "||Enable||DeliveryRequest ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "DeliveryRequest");
        }
    }
}
