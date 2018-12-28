using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.Web.Models;
using Application.Web.Models.ViewModels;
using RestSharp;
using Application.Web.Helper;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Application.Web.Controllers.FileMaintainance
{
    public class TransactionStatusController : BaseController
    {

         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: TransactionStatus
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new TransactionStatusDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<TransactionStatusViewModel>)result.filteredList;

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
                TransTypeCode = x.TransTypeCode,
                TransTypeDesc = x.TransTypeDesc,
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

        // GET: TransactionStatus/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new TransactionStatusViewModel();
            var url = "api/Status/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new TransactionStatusViewModel
                {
                    Id = data.Id,
                    Code = data.Code,
                    Name = data.Name,
                    TransactionTypeId = data.TransactionTypeId,
                    TransTypeCode = data.TransactionType != null ? data.TransactionType.Code : "",
                    TransTypeDesc = data.TransactionType != null ? data.TransactionType.TransType : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);

        }

        // GET: TransactionStatus/Create
        public ActionResult Create()
        {
            var obj = new TransactionStatusViewModel
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: TransactionStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, Code, Name, TransactionTypeId")] TransactionStatusViewModel obj)
        {
            if (ModelState.IsValid)
            {
                obj.CreatedBy = CookieHelper.EmailAddress;

                var url = "api/Status/Add";
                var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["Message"] = obj.Code + " is already exist! Please check and try again.";
                    TempData["MessageAlert"] = "warning";
                    return RedirectToAction("Index");
                }

                if (response.IsSuccessful)
                {
                    TempData["Message"] = obj.Name + " successfully created!";
                    TempData["MessageAlert"] = "success";
                    //Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Create||Transaction Status ID::{0}||API Response::{1}", transactionStatusViewModel.Id, response));
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Create||Transaction Status ID::{0}||API Response::{1}", obj.Id, response));
                    //return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index", "TransactionStatus");
            //return View(transactionStatusViewModel);
        }

        // GET: TransactionStatus/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new TransactionStatusViewModel();
            var url = "api/Status/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new TransactionStatusViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    CreatedBy = data.CreatedBy ?? "",
                    DateCreated = data.DateCreated ?? null,
                    Code = data.Code,
                    Name = data.Name,
                    TransactionTypeId = data.TransactionTypeId,
                    TransTypeCode = data.TransactionType != null ? data.TransactionType.Code : "",
                    TransTypeDesc = data.TransactionType != null ? data.TransactionType.TransType : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        // POST: TransactionStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, Code, Name, TransactionTypeId, IsActive, CreatedBy, DateCreated, UpdatedBy, DateUpdated")] TransactionStatusViewModel obj)
        {
            
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/status/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.Code + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Name + " successfully updated!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Update||Transaction Status ID::{0}||API Response::{1}", transactionStatusViewModel.Id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Update||Transaction Status ID::{0}||API Response::{1}", obj.Id, response));
                //return PartialView(transactionStatusViewModel);
            }
            return RedirectToAction("Index", "TransactionStatus");
            //return View(transactionStatusViewModel);
        }

        // GET: TransactionStatus/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            var obj = new TransactionStatusViewModel();
            var url = "api/Status/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new TransactionStatusViewModel
                {
                    Id = data.Id,
                    Code = data.Code,
                    Name = data.Name,
                    TransactionTypeId = data.TransactionTypeId,
                    TransTypeCode = data.TransactionType != null ? data.TransactionType.Code : "",
                    TransTypeDesc = data.TransactionType != null ? data.TransactionType.TransType : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        // POST: TransactionStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {

            var url = $"api/status/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Status successfully deleted!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Delete||TransactionStatus ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionStatus");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Delete||TransactionStatus ID::{0}||API Response::{1}", id, response));
            }
            return RedirectToAction("Index", "TransactionStatus");
        }

        public async Task<ActionResult> Enable(long? id)
        {
            var obj = new TransactionStatusViewModel();
            var url = "api/Status/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new TransactionStatusViewModel
                {
                    Id = data.Id,
                    Code = data.Code,
                    Name = data.Name,
                    TransactionTypeId = data.TransactionTypeId,
                    TransTypeCode = data.TransactionType != null ? data.TransactionType.Code : "",
                    TransTypeDesc = data.TransactionType != null ? data.TransactionType.TransType : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        // POST: TransactionStatus/Delete/5
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {

            var url = $"api/status/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Status successfully enabled!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Delete||TransactionStatus ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionStatus");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionStatusController).Name) + "||Enable||TransactionStatus ID::{0}||API Response::{1}", id, response));
            }
            return RedirectToAction("Index", "TransactionStatus");
        }
    }
}
