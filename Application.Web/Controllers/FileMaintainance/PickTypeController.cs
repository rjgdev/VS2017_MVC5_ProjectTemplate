using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Application.Web.Helper;
using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Web.Controllers.FileMaintainance
{
    public class PickTypeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: PickType
        public  async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new PickTypeDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<PickTypeViewModels>)result.filteredList;

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

        // GET: PickType/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var obj = new PickTypeViewModels();
            var url = "api/PickType/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new PickTypeViewModels
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    Code = data.Code,
                    Description = data.Description,
                    IsActive = data.IsActive,
                };

            }

            return PartialView(obj);
        }

        // GET: PickType/Create
        public ActionResult Create()
        {
            var obj = new PickTypeViewModels()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        // POST: PickType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, Code, Description")] PickTypeViewModels obj)
        {
            obj.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/PickType/Add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Description + " successfully created!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Item");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {

                TempData["Message"] = obj.Code + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(PickTypeController).Name) + "||Create||Pick Type ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index", "Item");
            }

            return RedirectToAction("Index", "PickType");
        }

        // GET: PickType/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var obj = new PickTypeViewModels();
            var url = "api/PickType/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new PickTypeViewModels
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    Code = data.Code,
                    Description = data.Description,
                    IsActive = data.IsActive,
                    CreatedBy = data.CreatedBy ?? "",
                    DateCreated = data.DateCreated ?? null
                };

            }

            return PartialView(obj);
        }

        // POST: PickType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, Code, Description, IsActive, CreatedBy, DateCreated")] PickTypeViewModels obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/PickType/Update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Description + " successfully updated!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Item");
            }
            //else if(responseMessage.StatusCode == HttpStatusCode.BadRequest)
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(PickTypeController).Name) + "||Edit||Pick Type ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index", "Item");
            }

            return RedirectToAction("Index", "PickType");
        }

        // GET: PickType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var obj = new PickTypeViewModels();
            var url = "api/PickType/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new PickTypeViewModels
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    Code = data.Code,
                    Description = data.Description,
                    IsActive = data.IsActive,
                };

            }

            return PartialView(obj);
        }

        // POST: PickType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var url = $"api/PickType/Delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Pick Type successfully deleted!";
                TempData["MessageAlert"] = "success";
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(PickTypeController).Name) + "||Delete||Pick Type ID::{0}||API Response::{1}", id, response));

            }

            return RedirectToAction("Index", "PickType");
        }

        // GET: PickType/Enable/5
        public async Task<ActionResult> Enable(int? id)
        {
            var obj = new PickTypeViewModels();
            var url = "api/PickType/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new PickTypeViewModels
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    Code = data.Code,
                    Description = data.Description,
                    IsActive = data.IsActive,
                };

            }

            return PartialView(obj);
        }

        // POST: PickType/Delete/5
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableConfirmed(int id)
        {
            var url = $"api/PickType/Enable/{id}/{_updatedBy}/";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Pick Type successfully enabled!";
                TempData["MessageAlert"] = "success";
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(PickTypeController).Name) + "||Enable||Pick Type ID::{0}||API Response::{1}", id, response));

            }

            return RedirectToAction("Index", "PickType");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
