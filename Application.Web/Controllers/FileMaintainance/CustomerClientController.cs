using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Web.Models.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;
using Application.Web.Helper;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using RestSharp;
using Application.Web.Models;

namespace Application.Web.Controllers.FileMaintainance
{
    public class CustomerClientController : BaseController
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: CustomerClient
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new CustomerClientDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<CustomerClientViewModel>)result.filteredList;

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
                CustomerCode = x.CustomerCode,
                Description = x.Name,
                ContactPerson = x.ContactPerson,
                Telephone = x.Telephone,
                MobileNo = x.MobileNo,
                EmailAddress = x.EmailAddress,
                Website = x.Website,
                Address1 = x.Address1,
                Address2 = x.Address2,
                IsActve = x.IsActive
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
            var obj = new CustomerClientViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, CustomerCode, Name, ContactPerson, Telephone, MobileNo, EmailAddress, Website, Address1, Address2")]CustomerClientViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/CustomerClient/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.CustomerCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Name + " successfully created!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerClientController).Name) + "||Create||Customer Client ID::{0}||API Response::{1}", obj.Id, response));
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            var obj = new CustomerClientViewModel();
            var url = "api/CustomerClient/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerClientViewModel>(result);
            }

            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var obj = new CustomerClientViewModel();
            var url = "api/CustomerClient/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerClientViewModel>(result);
            }

            return PartialView(obj);

        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, CustomerCode, Name, ContactPerson, Telephone, MobileNo, EmailAddress, Website, Address1, Address2, DateUpdated, UpdatedBy, " +
            "IsActive, DateCreated, CreatedBy")]CustomerClientViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/CustomerClient/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.CustomerCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                //return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Name + " successfully updated!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerClientController).Name) + "||Edit||ITem Group ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var obj = new CustomerClientViewModel();
            var url = "api/CustomerClient/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerClientViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var url = $"api/CustomerClient/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Customer succesfully deleted!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerClientViewModel).Name) + "||Delete||Customer Client ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index", "CustomerClient");
        }

        public async Task<ActionResult> Enable(int? id)
        {
            var obj = new CustomerClientViewModel();
            var url = "api/CustomerClient/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerClientViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id)
        {
            var url = $"api/CustomerClient/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Customer successfully enabled!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerClientViewModel).Name) + "||Enable||Customer Client ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index", "CustomerClient");
        }
    }
}