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
using System.Net.Http.Headers;
using RestSharp;

namespace Application.Web.Controllers.FileMaintainance
{
    public class TransactionTypesController : BaseController
    {

         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: TransactionType
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new TransactionTypeDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<TransactionTypesViewModel>)result.filteredList;

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
                TransType = x.TransType,
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

        // GET: TransactionType/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new TransactionTypesViewModel();
            var url = "api/transactiontypes/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<TransactionTypesViewModel>(result);
            }

            return PartialView(obj);
        }

        // GET: TransactionType/Create
        public ActionResult Create()
        {
            var obj = new TransactionTypesViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: TransactionType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, Code, TransType")] TransactionTypesViewModel obj)
        {

            //transactionTypeViewModel.Domain = "Capcom";
            obj.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/transactiontypes/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.Code + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.TransType + " successfully created!";
                TempData["MessageAlert"] = "success";

            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionTypesController).Name) + "||Create||TransactinType ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
            }
            
            return RedirectToAction("Index", "TransactionTypes");
        }

        // GET: TransactionType/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new TransactionTypesViewModel();
            var url = "api/transactiontypes/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<TransactionTypesViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: TransactionType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, Code, TransType, IsActive, CreatedBy, DateCreated")] TransactionTypesViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/transactiontypes/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.Code + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                //return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.TransType + " successfully updated!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(TransactionTypesController).Name) + "||Edit||Transaction Type ID::{0}||API Response::{1}", obj.Id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionTypesController).Name) + "||Edit||Transaction Type ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
            }

            return RedirectToAction("Index", "TransactionTypes");

        }

        // GET: Location/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            var obj = new TransactionTypesViewModel();
            var url = "api/transactiontypes/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<TransactionTypesViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: Product/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var url = $"api/transactiontypes/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Transaction Types successfully deleted!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionTypesController).Name) + "||Delete||Transaction Type ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index", "TransactionTypes");
        }

        // GET: TransactionTypes/Enable/5
        public async Task<ActionResult> Enable(long? id)
        {
            var obj = new TransactionTypesViewModel();
            var url = "api/transactiontypes/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<TransactionTypesViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: TransactionTypes/Enable/5
        [Authorize]
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {
            var url = $"api/transactiontypes/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Transaction Types successfully enabled!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(TransactionTypesController).Name) + "||Enable||Transaction Type ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index", "TransactionTypes");
        }

    }
}
