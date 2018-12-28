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
using RestSharp;
using Newtonsoft.Json;

namespace Application.Web.Controllers.FileMaintainance
{
    public class HaulierController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: Haulier
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new HaulierDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<HaulierViewModel>)result.filteredList;

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
                HaulierCode = x.HaulierCode,
                Description = x.Name,
                ContactPerson = x.ContactPerson,
                EmailAddress = x.EmailAddress,
                Website = x.Website,
                Telephone = x.Telephone,
                MobileNo = x.MobileNo,
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

        // GET: Haulier/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            var obj = new HaulierViewModel();
            var url = "api/haulier/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new HaulierViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    HaulierCode = data.HaulierCode,
                    Name = data.Name,
                    ContactPerson = data.ContactPerson,
                    Telephone = data.Telephone,
                    MobileNo = data.MobileNo,
                    EmailAddress = data.EmailAddress,
                    Website = data.Website,
                    IsActive = data.IsActive,
                    DateCreated = data.DateCreated,
                    CreatedBy = data.CreatedBy,
                };
            }

            return PartialView(obj);
        }

        // GET: Haulier/Create
        public ActionResult Create()
        {
            var obj = new HaulierViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: Haulier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, HaulierCode, Name, ContactPerson, Telephone, MobileNo, EmailAddress, Website, IsActive, DateCreated, CreatedBy")] HaulierViewModel obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/haulier/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.HaulierCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Name + " successfully created!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Create||Haulier ID::{0}||API Response::{1}", haulierViewModel.Id, response));
            }

            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Create||Haulier ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Haulier");
        }

        // GET: Haulier/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            var obj = new HaulierViewModel();
            var url = "api/haulier/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new HaulierViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    HaulierCode = data.HaulierCode,
                    Name = data.Name,
                    ContactPerson = data.ContactPerson,
                    Telephone = data.Telephone,
                    MobileNo = data.MobileNo,
                    EmailAddress = data.EmailAddress,
                    Website = data.Website,
                    IsActive = data.IsActive,
                    DateCreated = data.DateCreated,
                    CreatedBy = data.CreatedBy,
                };
            }

            return PartialView(obj);
        }

        // POST: Haulier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, HaulierCode, Name, ContactPerson, Telephone, MobileNo, EmailAddress, Website, IsActive, DateCreated, CreatedBy")] HaulierViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/haulier/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.HaulierCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Name + " successfully updated!";
                TempData["MessageAlert"] = "success";
                Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Update||Location ID::{0}||API Response::{1}", obj.Id, response));
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Update||Location ID::{0}||API Response::{1}", obj.Id, response));
                return PartialView(obj);
            }
        }

        // GET: Haulier/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            var obj = new HaulierViewModel();
            var url = "api/haulier/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new HaulierViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    HaulierCode = data.HaulierCode,
                    Name = data.Name,
                    ContactPerson = data.ContactPerson,
                    Telephone = data.Telephone,
                    MobileNo = data.MobileNo,
                    EmailAddress = data.EmailAddress,
                    Website = data.Website,
                    IsActive = data.IsActive,
                    DateCreated = data.DateCreated,
                    CreatedBy = data.CreatedBy,
                };
            }

            return PartialView(obj);
        }

        // POST: Haulier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var url = $"api/haulier/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Haulier successfully deleted!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Delete||Haulier ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Delete||Haulier ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Haulier");
        }

        // GET: Haulier/Delete/5
        public async Task<ActionResult> Enable(long? id)
        {
            var obj = new HaulierViewModel();
            var url = "api/haulier/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new HaulierViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    HaulierCode = data.HaulierCode,
                    Name = data.Name,
                    ContactPerson = data.ContactPerson,
                    Telephone = data.Telephone,
                    MobileNo = data.MobileNo,
                    EmailAddress = data.EmailAddress,
                    Website = data.Website,
                    IsActive = data.IsActive,
                    DateCreated = data.DateCreated,
                    CreatedBy = data.CreatedBy,
                };
            }

            return PartialView(obj);
        }

        // POST: Haulier/Delete/5
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(long id)
        {
            var url = $"api/haulier/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Haulier successfully enabled!";
                TempData["MessageAlert"] = "success";
                //Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Delete||Haulier ID::{0}||API Response::{1}", id, response));
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(HaulierController).Name) + "||Enable||Haulier ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Haulier");
        }
    }
}
