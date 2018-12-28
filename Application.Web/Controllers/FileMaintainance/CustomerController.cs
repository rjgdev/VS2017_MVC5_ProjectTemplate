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
using System.IO;
using Application.Web.Models;

namespace Application.Web.Controllers
{
    public class CustomerController : BaseController
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: Customer
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new CustomerDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<CustomerViewModel>)result.filteredList;

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
                                (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Disable</a></li>" :
                                              "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>") +
                            "</ul>" +
                          "</div>",
                FullName = x.FirstName + (!string.IsNullOrEmpty(x.MiddleName) ? " " + x.MiddleName[0] + ". " : " " ) + x.LastName,
                EmailAddress = x.EmailAddress,
                Domain = x.Domain,
                CompanyName = x.CompanyName,
                ContactNo = x.ContactNo,
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
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "CompanyName, FirstName, LastName, MiddleName, Domain, EmailAddress, IsActive, DateCreated, CreatedBy, ContactNo")]CustomerViewModel customer)
        {
            customer.CreatedBy = CookieHelper.EmailAddress;
            customer.IsActive = true;

            //Encode the string.
            //var myEncodedString = HttpUtility.HtmlEncode(customer.CompanyName);

            //StringWriter myWriter = new StringWriter();

            //Decode the encoded string.
           //HttpUtility.HtmlDecode(myEncodedString, myWriter);
            //var badString = customer.CompanyName;
            //var data = new HtmlAndXmlWriter();

            //var x = await data.Escape(badString);
            //var obj = x;

            //var y =  data.GetHtmlFromOutObject(obj);

            var url = "api/customer/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, customer);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = customer.EmailAddress + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = customer.CompanyName + " successfully created!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerController).Name) + "||Create||Customer ID::{0}||API Response::{1}", customer.Id, response));
                return PartialView(customer);
                //return RedirectToAction("Index", "Customer");
            }
            return RedirectToAction("Index", "Customer");
        }

        public async Task<ActionResult> Details(int id)
        {
            var obj = new CustomerViewModel();
            var url = "api/Customer/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerViewModel>(result);
            }

            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var obj = new CustomerViewModel();
            var url = "api/Customer/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CompanyName, FirstName, LastName, MiddleName, Domain, EmailAddress, DateCreated, CreatedBy, UpdatedBy, DateUpdated, IsActive, ContactNo")]CustomerViewModel customer)
        {
            customer.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/Customer/Update/";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, customer);
            
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = customer.EmailAddress + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                //return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = customer.CompanyName + " successfully updated!";
                TempData["MessageAlert"] = "success";
            }
            //else if(responseMessage.StatusCode == HttpStatusCode.BadRequest)
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerController).Name) + "||Edit||Customer ID::{0}||API Response::{1}", customer.Id, response));
                return PartialView(customer);
            }

            return RedirectToAction("Index", "Customer");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var obj = new CustomerViewModel();
            var url = "api/Customer/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerViewModel>(result);
            }

            ViewBag.CompanyName = obj.CompanyName;

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, string CompanyName)
        {
            var url = $"api/Customer/delete/{id}/{_updatedBy}/";
            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Company successfully disabled!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Customer");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerController).Name) + "||Delete||Customer ID::{0}||API Response::{1}", id, response));
            }

            return RedirectToAction("Index", "Customer");
        }

        public async Task<ActionResult> Enable(int? id)
        {
            var obj = new CustomerViewModel();
            var url = "api/Customer/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<CustomerViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id)
        {
            var url = $"api/Customer/enable/{id}/{_updatedBy}/";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Company successfully enabled!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Customer");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(CustomerController).Name) + "||Enable||Customer ID::{0}||API Response::{1}", id, response));
            }

            return RedirectToAction("Index", "Customer");
        }

        
    }
}