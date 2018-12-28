using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Application.Web.Models.ViewModels;
using System.Net.Http;
using Application.Web.Helper;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RestSharp;
using Application.Web.Models;
using System.Linq;

namespace Application.Web.Controllers.FileMaintainance
{
    public class VendorController : BaseController
    {

         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: VendorViewModels
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new VendorDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<VendorViewModel>)result.filteredList;

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
                VendorCode = x.VendorCode,
                VendorName = x.VendorName,
                ContactPerson = x.ContactPerson,
                Telephone = x.Telephone,
                MobileNo = x.MobileNo,
                EmailAddress =  x.EmailAddress,
                Website = x.Website,
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

        // GET: VendorViewModels/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new VendorViewModel();
            var url = "api/vendor/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<VendorViewModel>(result);
            }

            return PartialView(obj);
        }

        // GET: VendorViewModels/Create
        public  ActionResult Create()
        {
            var obj = new VendorViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: VendorViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, VendorCode, VendorName, ContactPerson, Telephone, MobileNo, EmailAddress, Website, Address1, Address2, CreatedBy, DateCreated, IsActive")] VendorViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/vendor/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.VendorCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.VendorName + " successfully created!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Create||Vendor ID::{0}||API Response::{1}", vendorViewModel.Id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Create||Vendor ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        // GET: VendorViewModels/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new VendorViewModel();
            var url = "api/Vendor/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<VendorViewModel>(result);
            }

            return PartialView(obj);
        }

        // POST: VendorViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, VendorCode, VendorName, , ContactPerson, Telephone, MobileNo, EmailAddress, Website, Address1, Address2, UpdatedBy, DateUpdated, CreatedBy, DateCreated, IsActive")] VendorViewModel vendorViewModel)
        {
            vendorViewModel.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/vendor/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT,vendorViewModel);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = vendorViewModel.VendorCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = vendorViewModel.VendorName + " successfully updated!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Update||Vendor ID::{0}||API Response::{1}", vendorViewModel.Id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Update||Vendor ID::{0}||API Response::{1}", vendorViewModel.Id, response));
                return PartialView(vendorViewModel);
            }

            return RedirectToAction("Index", "Vendor");

        }

        // GET: Location/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new VendorViewModel();
            var url = "api/vendor/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<VendorViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: Product/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var url = $"api/vendor/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Vendor successfully deleted!";
                TempData["MessageAlert"] = "success";
                Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Delete||Vendor ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Delete||Vendor ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index");
            }

        }

        // GET: Location/Delete/5
        [Authorize]
        public async Task<ActionResult> Enable(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new VendorViewModel();
            var url = "api/vendor/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<VendorViewModel>(result);
            }

            return PartialView(obj);

        }

        // POST: Product/Delete/5
        [Authorize]
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {
            var url = $"api/Vendor/Enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Vendor successfully enabled!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Enable||Vendor ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(VendorController).Name) + "||Enable||Vendor ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
