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
using Application.Web.Helper;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Web.Controllers.Transaction
{
    public class DeliveryRequestLineController : BaseController
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: DeliveryRequestLineModels
        [Authorize]
        public async Task<ActionResult> Index(long? id)
        {
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
                    Priority = data.Priority,
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

                ViewBag.DeliveryRequestId = obj.Id;
                ViewBag.StatusId = obj.StatusId;

                return View();
            }

            return View("Index", "DeliveryRequest");
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterLineViewModels request)
        {
            var helper = new DeliveryRequestLineDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<DeliveryRequestLineViewModel>)result.filteredList;

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
                                              (x.StatusId != 15 ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>": "")) +
                            "</ul>" +
                          "</div>",
                PickType = x.PickType,
                ProductDescription = x.ProductDescription,
                Brand = x.Brand,
                ItemDescription = x.ItemDescription,
                UomDescription = x.UomDescription,
                Quantity = x.Quantity,
            });

            return Json(new
            {
                draw = request.draw,
                recordsTotal = result.totalResultCount,
                recordsFiltered = result.filteredResultCount,
                data = data
            });
        }

        // GET: DeliveryRequestLineModels/Details/5
        [Authorize]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineViewModel();
            var url = "api/GoodsOut/DeliveryRequest/Line/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<DeliveryRequestLineViewModel>(result);
            }

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestLineViewModel
                {
                    Id = data.Id,
                    PickType = data.PickType ?? "",
                    ProductDescription = data.Product ?? "",
                    Brand = data.Brand ?? "",
                    ItemDescription = data.Item ?? "",
                    UomDescription = data.Uom ?? "",
                    Quantity = data.Quantity ?? 0,
                    Memo = data.Memo ?? "",
                    SpecialInstructions = data.SpecialInstructions ?? "",
                    IsActive = data.IsActive
                };

            }

            return PartialView(obj);
        }

        // GET: DeliveryRequestLineModels/Create
        [Authorize]
        public ActionResult Create(long id)
        {
            var obj = new DeliveryRequestLineViewModel()
            {
                DeliveryRequestId = id,
                CustomerId = int.Parse(CookieHelper.CustomerId),
                Quantity = 1
            };

            return PartialView(obj);
        }

        // POST: DeliveryRequestLineModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DeliveryRequestId, ProductId, PickTypeId, Quantity," +
            "SpecialInstructions, Memo, BrandId, ItemId, UomId, CustomerId")] DeliveryRequestLineViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;
            var url = "api/GoodsOut/DeliveryRequest/Line/Add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Created";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Create||DeliveryRequestLine ID::{0}||API Response::{1}", deliveryRequestLineViewModel.Id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Create||DeliveryRequestLine ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index", new { id = deliveryRequestLineViewModel.DeliveryRequestId });
            }

            return RedirectToAction("Index", new { id = obj.DeliveryRequestId });
        }

        // GET: DeliveryRequestLineModels/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineViewModel();
            var url = "api/GoodsOut/DeliveryRequest/Line/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestLineViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    DeliveryRequestId = data.DeliveryRequestId,
                    PickTypeId = data.PickTypeId,
                    PickType = data.PickType ?? "",
                    ProductId = data.ProductId,
                    ProductDescription = data.Product ?? "",
                    BrandId = data.BrandId,
                    Brand = data.Brand ?? "",
                    ItemId = data.ItemId,
                    ItemDescription = data.Item ?? "",
                    UomId = data.UomId,
                    UomDescription = data.Uom ?? "",
                    Quantity = data.Quantity ?? 0,
                    Memo = data.Memo ?? "",
                    SpecialInstructions = data.SpecialInstructions ?? "",
                    IsActive = data.IsActive,
                    CreatedBy = data.CreatedBy,
                    DateCreated = data.DateCreated
                };

            }

            return PartialView(obj);
        }

        // POST: DeliveryRequestLineModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, DeliveryRequestId, ProductId, BrandId," +
            "ItemId, PickTypeId, UomId, Quantity, SpecialInstructions, Memo, IsActive, CreatedBy, DateCreated")] DeliveryRequestLineViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;
            var url = "api/GoodsOut/DeliveryRequest/Line/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Updated";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Update||DeliveryRequestLine ID::{0}||API Response::{1}", obj.Id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Update||DeliveryRequestLine ID::{0}||API Response::{1}", obj.Id, response));
                //return PartialView(obj);
            }

            return RedirectToAction("Index", new { id = obj.DeliveryRequestId });
        }

        // GET: DeliveryRequestLineModels/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineViewModel();
            var url = "api/GoodsOut/DeliveryRequest/Line/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestLineViewModel
                {
                    Id = data.Id,
                    DeliveryRequestId = data.DeliveryRequestId,
                    PickType = data.PickType ?? "",
                    ProductDescription = data.Product ?? "",
                    Brand = data.Brand ?? "",
                    ItemDescription = data.Item ?? "",
                    UomDescription = data.Uom ?? "",
                    Quantity = data.Quantity ?? 0,
                    Memo = data.Memo ?? "",
                    SpecialInstructions = data.SpecialInstructions ?? "",
                    IsActive = data.IsActive
                };

            }

            return PartialView(obj);
        }

        // POST: DeliveryRequestLineModels/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(DeliveryRequestLineViewModel obj, long id)
        {

            //var deliveryRequestId = Request.Form["DeliveryRequestId"]; //FormCollection["DeliveryRequestId"];
            var url = $"api/GoodsOut/DeliveryRequest/Line/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Deleted";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Delete||DeliveryRequestLine ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Delete||DeliveryRequestLine ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", new { id = DeliveryRequestId });
            }

            return RedirectToAction("Index", new { id = obj.DeliveryRequestId });
        }

        // GET: DeliveryRequestLineModels/Delete/5
        [Authorize]
        public async Task<ActionResult> Enable(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineViewModel();
            var url = "api/GoodsOut/DeliveryRequest/Line/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new DeliveryRequestLineViewModel
                {
                    Id = data.Id,
                    DeliveryRequestId = data.DeliveryRequestId,
                    PickType = data.PickType ?? "",
                    ProductDescription = data.Product ?? "",
                    Brand = data.Brand ?? "",
                    ItemDescription = data.Item ?? "",
                    UomDescription = data.Uom ?? "",
                    Quantity = data.Quantity ?? 0,
                    Memo = data.Memo ?? "",
                    SpecialInstructions = data.SpecialInstructions ?? "",
                    IsActive = data.IsActive
                };

            }

            return PartialView(obj);
        }

        // POST: DeliveryRequestLineModels/Delete/5
        [Authorize]
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(DeliveryRequestLineViewModel obj, long id)
        {

            //var deliveryRequestId = Request.Form["DeliveryRequestId"];
            var url = $"api/GoodsOut/DeliveryRequest/Line/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Enabled";
                //Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Enable||DeliveryRequestLine ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineController).Name) + "||Delete||DeliveryRequestLine ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", new { id = DeliveryRequestId });
            }

            return RedirectToAction("Index", new { id = obj.DeliveryRequestId });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetItemByItemId(long? id)
        {
            var url = "api/Item/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                return Json(response.Content);
            }

            return null;
        }
    }
}
