using Application.Web.Helper;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using RestSharp;
using Application.Web.Models;

namespace Application.Web.Controllers
{
    public class WarehouseController : BaseController
    {
        // GET: Warehouse
        private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new WarehouseDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<WarehouseViewModel>)result.filteredList;

            var data = resultList.Select(x => new
            {
                Actions = "<div class='btn-group' style='display:flex;'>" +
                            "<a class='btn btn-primary' href='#'><i class='fa fa-gear fa-fw'></i> Action</a>" +
                            "<a class='btn btn-primary dropdown-toggle' data-toggle='dropdown' href='#'>" +
                                "<span class='fa fa-caret-down' title='Toggle dropdown menu'></span>" +
                            "</a>" +
                            "<ul class='dropdown-menu' role='menu'>" +
                                (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" : "") +
                                "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                                (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Delete</a></li>" :
                                              "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>") +
                            "</ul>" +
                          "</div>",
                WarehouseCode = x.WarehouseCode,
                Description = x.Description,
                Address1 = x.Address1,
                Address2 = x.Address2,
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

        [Authorize]
        public ActionResult Create()
        {
            var obj = new WarehouseViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, WarehouseCode, Description, Address1, Address2")]WarehouseViewModel obj)
        {
            //obj.Domain = CookieHelper.Domain;
            obj.CreatedBy = CookieHelper.EmailAddress;


            var url = "api/warehouse/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.WarehouseCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Description + " successfully created!";
                TempData["MessageAlert"] = "success";
                Log.Error(string.Format(Type.GetType(typeof(WarehouseController).Name) + "||Create||Warehouse ID::{0}||API Response::{1}", obj.Id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(WarehouseController).Name) + "||Create||Warehouse ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Warehouse");
        }

        [Authorize]
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new WarehouseViewModel();
            var url = "api/warehouse/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<WarehouseViewModel>(result);
            }

            return PartialView(obj);
        }

        [Authorize]
        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new WarehouseViewModel();
            var url = "api/warehouse/getbyid/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<WarehouseViewModel>(result);
            }
          

            return PartialView(obj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, WarehouseCode, Description, IsActive, Address1, Address2, DateCreated, CreatedBy, UpdatedBy, DateUpdated")]WarehouseViewModel obj)
        {

            //warehouse.Domain = CookieHelper.Domain;
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/warehouse/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.WarehouseCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                //return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Description + " successfully updated!";
                TempData["MessageAlert"] = "success";
                Log.Error(string.Format(Type.GetType(typeof(WarehouseController).Name) + "||Update||Warehouse ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index", "Warehouse");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(WarehouseController).Name) + "||Update||Warehouse ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
            }
        return RedirectToAction("Index", "Warehouse");
        }

        [Authorize]
        // GET: Location/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {

            var obj = new WarehouseViewModel();
            var url = "api/warehouse/getbyid/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<WarehouseViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: Product/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {

            var url = $"api/warehouse/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Warehouse successfully deleted!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Warehouse");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(WarehouseController).Name) + "||Delete||Warehouse ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Warehouse");
        }

        [Authorize]
        // GET: Location/Delete/5
        public async Task<ActionResult> Enable(long? id)
        {

            var obj = new WarehouseViewModel();
            var url = "api/warehouse/getbyid/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<WarehouseViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: Product/Delete/5
        [Authorize]
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {

            var url = $"api/warehouse/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Warehouse successfully enabled!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Warehouse");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(WarehouseController).Name) + "||Enable||Warehouse ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Warehouse");
        }
    }
}