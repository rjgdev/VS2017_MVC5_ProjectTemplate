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
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Web.Controllers.FileMaintainance
{
    public class BrandController : BaseController
    {
        private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: BrandViewModels
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new BrandDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<BrandViewModel>)result.filteredList;

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
                Name = x.Name,
                ProductCode = x.ProductCode,
                ProductDesc = x.ProductDesc,
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

        // GET: BrandViewModels/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new BrandViewModel();
            var url = "api/brand/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new BrandViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductCode = data.Product?.ProductCode ?? "",
                    ProductDesc = data.Product?.Description ?? "",
                    Code = data.Code,
                    Name = data.Name,
                    IsActive = data.IsActive,
                };

            }

            return PartialView(obj);
        }

        // GET: BrandViewModels/Create
        public ActionResult Create()
        {
            var obj = new BrandViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: BrandViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id, CustomerId, Code, Name, ProductId, ProductCode, IsActive")] BrandViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;

            if (ModelState.IsValid)
            {
                var url = "api/brand/add";
                var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["Message"] = obj.Code  + " is already exist! Please check and try again.";
                    TempData["MessageAlert"] = "warning";
                    return RedirectToAction("Index");
                }

                if (response.IsSuccessful)
                {
                    TempData["Message"] = obj.Name + " successfully created!";
                    TempData["MessageAlert"] = "success";
                    //Log.Error(string.Format(Type.GetType(typeof(BrandViewModel).Name) + "||Create||Brand ID::{0}||API Response::{1}", obj.Id, response));
                    //return RedirectToAction("Index");
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(BrandViewModel).Name) + "||Create||Brand ID::{0}||API Response::{1}", obj.Id, response));
                    //return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        // GET: BrandViewModels/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new BrandViewModel();
            var url = "api/brand/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new BrandViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductId = data.Product?.Id ?? null,
                    ProductCode = data.Product?.ProductCode ?? "",
                    ProductDesc = data.Product?.Description ?? "",
                    Code = data.Code,
                    Name = data.Name,
                    IsActive = data.IsActive,
                    DateCreated = data.DateCreated ?? null,
                    CreatedBy = data.CreatedBy ?? ""
                };

            }

            return PartialView(obj);
        }

        // POST: BrandViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, Code, Name, IsActive, ProductId, IsActive, DateCreated, CreatedBy")]BrandViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            if (ModelState.IsValid)
            {
                var url = "api/brand/update";
                var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["Message"] = obj.Code + " is already exist! Please check and try again.";
                    TempData["MessageAlert"] = "warning";
                }

                if (response.IsSuccessful)
                {
                    TempData["Message"] = obj.Name + " successfully updated!";
                    TempData["MessageAlert"] = "success";
                    //Log.Error(string.Format(Type.GetType(typeof(BaseController).Name) + "||Update||Bases ID::{0}||API Response::{1}", obj.Id, response));
                    //return RedirectToAction("Index");
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(BaseController).Name) + "||Update||Bases ID::{0}||API Response::{1}", obj.Id, response));
                    //return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        // GET: BrandViewModels/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            var obj = new BrandViewModel();
            var url = "api/brand/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new BrandViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductCode = data.Product?.ProductCode ?? "",
                    ProductDesc = data.Product?.Description ?? "",
                    Code = data.Code,
                    Name = data.Name,
                    IsActive = data.IsActive,
                };
            }

            return PartialView(obj);
        }

        // POST: BrandViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var url = $"api/brand/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "successfully deleted!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(BrandController).Name) + "||Delete||Brand ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(BrandController).Name) + "||Delete||Brand ID::{0}||API Response::{1}", id, response));
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Enable(long? id)
        {
            var obj = new BrandViewModel();
            var url = "api/brand/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new BrandViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductCode = data.Product?.ProductCode ?? "",
                    ProductDesc = data.Product?.Description ?? "",
                    Code = data.Code,
                    Name = data.Name,
                    IsActive = data.IsActive,
                };
            }

            return PartialView(obj);
        }

        // POST: BrandViewModels/Delete/5
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {
            var url = $"api/brand/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "successfully enabled!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(BrandController).Name) + "||Enable||Brand ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(BrandController).Name) + "||Enable||Brand ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
