using Application.Web.Models.ViewModels;
using Application.Web.Helper;
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
    public class ProductController : BaseController
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: Product
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new ProductDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<ProductViewModel>)result.filteredList;

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
                ProductCode = x.ProductCode,
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

        public async Task<ActionResult> Create()
        {
            var obj = new ProductViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, UomId, Description, ProductCode")]ProductViewModel product)
        {
            product.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/Product/Add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, product);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = product.ProductCode + " is already exist! Please check your Item Group Code and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index", "Product");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = product.Description + " successfully created!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ProductController).Name) + "||Create||Item Group ID::{0}||API Response::{1}", product.Id, response));
                //return RedirectToAction("Index", "Uom");
            }
            return RedirectToAction("Index", "Product");
        }

        public async Task<ActionResult> Details(int? id)
        {
            var obj = new ProductViewModel();
            var url = "api/Product/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<ProductViewModel>(result);
            }

            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var obj = new ProductViewModel();
            var url = "api/Product/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<ProductViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, Description, ProductCode, IsActive, DateCreated, CreatedBy")]ProductViewModel product)
        {
            product.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/Product/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, product);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = product.ProductCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                //return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = product.Description + " successfully updated!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ProductController).Name) + "||Edit||ITem Group ID::{0}||API Response::{1}", product.Id, response));
                return PartialView(product);
            }

            return RedirectToAction("Index", "Product");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var obj = new ProductViewModel();
            var url = "api/Product/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<ProductViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var url = $"api/Product/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Item Group successfully deleted!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ProductController).Name) + "||Delete||Item Group ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index", "Product");
        }

        public async Task<ActionResult> Enable(int? id)
        {
            var obj = new ProductViewModel();
            var url = "api/Product/GetById/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<ProductViewModel>(result);
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id)
        {
            var url = $"api/Product/enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Item Group successfully enabled!";
                TempData["MessageAlert"] = "success";
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ProductController).Name) + "||Enable||Item Group ID::{0}||API Response::{1}", id, response));
                //return RedirectToAction("Index", "TransactionTypes");
            }
            return RedirectToAction("Index", "Product");
        }
    }
}