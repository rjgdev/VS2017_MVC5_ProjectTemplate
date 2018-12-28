using Application.Web.Helper;
using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Application.Web.Controllers.FileMaintainance
{
    public class UomController : BaseController
    {
        // GET: Uom
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new UomDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<UomViewModel>)result.filteredList;

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
                Code = x.Code,
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

        public ActionResult Create()
        {
            var obj = new UomViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };
            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, Code, Description")]UomViewModel uom)
        {
            uom.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/Uom/Add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, uom);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = uom.Code + " code is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index", "Uom");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = uom.Description + " successfully created!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(UomController).Name) + "||Create||Uom ID::{0}||API Response::{1}", uom.Id, response));
                //return RedirectToAction("Index", "Uom");
            }
            return RedirectToAction("Index", "Uom");
        }

        public async Task<ActionResult> Details(int id)
        {
            var obj = new UomViewModel();
            var url = "api/Uom/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<UomViewModel>(result);
            }

            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new UomViewModel();
            var url = "api/Uom/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<UomViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, Code, Description, IsActive, DateCreated, CreatedBy")]UomViewModel uom)
        {
            uom.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/Uom/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, uom);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = uom.Code + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                //return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = uom.Description + " successfully updated!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(TransactionTypesController).Name) + "||Edit||Uom ID::{0}||API Response::{1}", uom.Id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(UomController).Name) + "||Edit||Uom ID::{0}||API Response::{1}", uom.Id, response));
                return PartialView(uom);
            }

            return RedirectToAction("Index", "Uom");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var obj = new UomViewModel();
            var url = "api/Uom/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<UomViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var url = $"api/Uom/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Uom successfully deleted!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(UomController).Name) + "||Delete||Uom ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Enable(int? id)
        {
            var obj = new UomViewModel();
            var url = "api/Uom/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<UomViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id)
        {
            var url = $"api/Uom/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Uom successfully enabled!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(UomController).Name) + "||Delete||Uom ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index");
        }
    }
}