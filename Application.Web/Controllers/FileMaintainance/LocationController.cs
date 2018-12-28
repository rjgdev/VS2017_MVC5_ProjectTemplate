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
using System.Net.Http;
using Application.Web.Helper;
using System.Net.Http.Headers;
using RestSharp;

namespace Application.Web.Controllers.FileMaintainance
{
    public class LocationController : BaseController
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;
        // GET: Location
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new LocationDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<LocationViewModel>)result.filteredList;

            var data = resultList.Select(x => new
            {
                Actions = "<div class='btn-group' style='display:flex;'>" +
                            "<a class='btn btn-primary' href='#'><i class='fa fa-gear fa-fw'></i> Action</a>" +
                            "<a class='btn btn-primary dropdown-toggle' data-toggle='dropdown' href='#'>" +
                                "<span class='fa fa-caret-down' title='Toggle dropdown menu'></span>" +
                            "</a>" +
                            "<ul class='dropdown-menu' role='menu'>" +
                                ( x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" : "") +
                                "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                                (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Delete</a></li>" :
                                              "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>") +
                            "</ul>" +
                          "</div>",
                //Order = x.Order,
                WarehouseCode = x.WarehouseCode,
                WarehouseDescription = x.WarehouseDescription,
                LocationCode = x.LocationCode,
                Description = x.Description,
                IsActive = x.IsActive
            });

            return Json(new
            {
                draw = request.draw,
                recordsTotal = result.totalResultCount,
                recordsFiltered = result.filteredResultCount,
                data = data
            });
        }

        // GET: Location/Details/5
        [Authorize]
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new LocationViewModel();
            var url = "api/location/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new LocationViewModel
                {
                    Id = data.Id,
                    Order = data.Order ?? 0,
                    Description = data.Description ?? "",
                    LocationCode = data.LocationCode ?? "",
                    WarehouseId = data.WarehouseId,
                    WarehouseCode = data.Warehouse != null ? data.Warehouse.WarehouseCode : "",
                    WarehouseDescription = data.Warehouse != null ? data.Warehouse.Description : "",
                    IsActive = data.IsActive
                };

            }

            return PartialView(obj);
        }

        // GET: Location/Create
        [Authorize]
        public ActionResult Create()
        {
            var obj = new LocationViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, LocationCode, Description, WarehouseId, Order, IsActive, DateCreated, CreatedBy")] LocationViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;

                var url = "api/location/add";
                var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["Message"] = obj.Description + " is already exist! Please check and try again.";
                    TempData["MessageAlert"] = "warning";
                }

                else if (response.IsSuccessful)
                {
                    TempData["Message"] = obj.Description + " successfully created!";
                    TempData["MessageAlert"] = "success";
                    //Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Create||Location ID::{0}||API Response::{1}", locationViewModel.Id, response));
                    //return RedirectToAction("Index");
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Create||Location ID::{0}||API Response::{1}", obj.Id, response));
                    return PartialView(obj);
                    //return RedirectToAction("Index");
                }
            return RedirectToAction("Index", "Location");
        }

        // GET: Location/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(long? id)
        {

            var obj = new LocationViewModel();
            var url = "api/location/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new LocationViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    Order = data.Order ?? 0,
                    Description = data.Description ?? "",
                    LocationCode = data.LocationCode ?? "",
                    WarehouseId = data.WarehouseId,
                    WarehouseCode = data.Warehouse != null ? data.Warehouse.WarehouseCode : "",
                    WarehouseDescription = data.Warehouse != null ? data.Warehouse.Description : "",
                    IsActive = data.IsActive,
                    CreatedBy = data.CreatedBy ?? "",
                    DateCreated = data.DateCreated ?? null
                };

            }

            return PartialView(obj);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, LocationCode, WarehouseId, Description, Order, IsActive, DateCreated, CreatedBy, UpdatedBy, DateCreated")] LocationViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;
          
                var url = "api/location/update";
                var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["Message"] = obj.Description + " is already exist! Please check and try again.";
                    TempData["MessageAlert"] = "warning";
                     return RedirectToAction("Index", "Location");
                }

                else if(response.IsSuccessful)
                {
                    TempData["Message"] = obj.Description + " successfully updated!";
                    TempData["MessageAlert"] = "success";
                    //Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Update||Location ID::{0}||API Response::{1}", locationViewModel.Id, response));
                    //return RedirectToAction("Index");
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Update||Location ID::{0}||API Response::{1}", obj.Id, response));
                    return PartialView(obj);
                    //return RedirectToAction("Index");
                }
            return RedirectToAction("Index", "Location");
        }

        // GET: Location/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(long? id)
        {

            var obj = new LocationViewModel();
            var url = "api/location/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new LocationViewModel
                {
                    Id = data.Id,
                    Order = data.Order ?? 0,
                    Description = data.Description ?? "",
                    LocationCode = data.LocationCode ?? "",
                    WarehouseId = data.WarehouseId,
                    WarehouseCode = data.Warehouse != null ? data.Warehouse.WarehouseCode : "",
                    WarehouseDescription = data.Warehouse != null ? data.Warehouse.Description : "",
                    IsActive = data.IsActive
                };

            }

            return PartialView(obj);

        }

        // POST: Location/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {

            var url = $"api/location/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Location successfully deleted!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Delete||Location ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Delete||Location ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: Location/Enable/5
        [Authorize]
        public async Task<ActionResult> Enable(long? id)
        {

            var obj = new LocationViewModel();
            var url = "api/location/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new LocationViewModel
                {
                    Id = data.Id,
                    Order = data.Order ?? 0,
                    Description = data.Description ?? "",
                    LocationCode = data.LocationCode ?? "",
                    WarehouseId = data.WarehouseId,
                    WarehouseCode = data.Warehouse != null ? data.Warehouse.WarehouseCode : "",
                    WarehouseDescription = data.Warehouse != null ? data.Warehouse.Description : "",
                    IsActive = data.IsActive
                };

            }

            return PartialView(obj);

        }

        // POST: Location/Delete/5
        [Authorize]
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {

            var url = $"api/location/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Location successfully enabled!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Enable||Location ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(LocationController).Name) + "||Enable||Location ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
