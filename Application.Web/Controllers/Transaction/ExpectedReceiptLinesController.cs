using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Web.Models.ViewModels;
using Application.Web.Helper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using RestSharp;
using Application.Web.Models;
using static Application.Web.Models.ViewModels.ExpectedReceiptViewModel;

namespace Application.Web.Controllers.Transaction
{
    public class ExpectedReceiptLinesController : BaseController
    {

         private int _customerId = int.Parse(CookieHelper.CustomerId);
         private string _updatedBy = CookieHelper.EmailAddress;

        // GET: ExpectedReceiptLines
        [Authorize]
        public async Task<ActionResult> Index(int? id, string GoodsReceivedNumber, string StatusCode, bool? IsActive)
        {

            ViewBag.ExpectedReceiptId = id;
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;
            ViewBag.IsAct = IsActive;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterLineViewModels request, bool? Planned, string StatusCode)
        {
            var helper = new ExpectedReceiptLineDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<ExpectedReceiptViewModel.ExpectedReceiptLine>)result.filteredList;

            var data = resultList.Select(x => new
            {

                Actions = "<div class='btn-group' style='display:flex;'>" +
                            "<a class='btn btn-primary' href='#'><i class='fa fa-gear fa-fw'></i> Action</a>" +
                            "<a class='btn btn-primary dropdown-toggle' data-toggle='dropdown' href='#'>" +
                                "<span class='fa fa-caret-down' title='Toggle dropdown menu'></span>" +
                            "</a>" +
                            "<ul class='dropdown-menu' role='menu'>" +
                            (x.StatusCode.Equals("For Receiving", StringComparison.OrdinalIgnoreCase) ?
                             //"<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" +
                             // "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                               "<li class='crud' data-id='" + x.Id + "' data-status='" + x.StatusCode + "' data-grn='" + x.GoodsReceivedNumber + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" +
                                    "<li class='crud' data-id='" + x.Id + "' data-status='" + x.StatusCode + "' data-grn='" + x.GoodsReceivedNumber + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                              (x.IsActive ?  "<li data-target = 'Delete' class='crud' data-id=" + x.Id + " data-status='" + x.StatusCode + "' data-grn='" + x.GoodsReceivedNumber + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Delete</a></li>" :
                                            "<li class='crud' data-id='" + x.Id + "' data-status='" + x.StatusCode + "' data-grn='" + x.GoodsReceivedNumber + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>") : "") +

                                 (x.StatusCode.Equals("Received", StringComparison.OrdinalIgnoreCase) || x.StatusCode.Equals("Completed", StringComparison.OrdinalIgnoreCase) ?
                                 "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" : "")+
                            "</ul>" +
                          "</div>",

                ExpiryDate = x.ExpiryDate?.ToString("MMM. dd, yyyy") ?? "",
                ItemCode = x.ItemCode,
                BrandName = x.BrandName,
                ItemDescription = x.ItemDescription,
                Quantity = x.Quantity,
                UomDescription = x.UomDescription,
                IsActive = x.IsActive
            });

            return Json(new
            {
                draw = request.draw,
                recordsTotal = result.totalResultCount,
                recordsFiltered = result.filteredResultCount,
                data = data
            });
            //return View(list);
        }

        public async Task<ActionResult> Create(int id, string GoodsReceivedNumber, string StatusCode) 
        {
            var obj = new ExpectedReceiptViewModel.ExpectedReceiptLine()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId),
                ExpectedReceiptId = id,
                GoodsReceivedNumber = GoodsReceivedNumber,
                StatusCode = StatusCode
            };

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "ExpectedReceiptId, CustomerId ProductId, UomId, CustomerId, Line, ProductCode, ItemDescription, ItemCode, ItemId, Batch, ExpiryDate," +
            " Quantity,UomDescription, BrandId, BrandName, IsChecked, ReferenceNumber, GoodsReceivedNumber, StatusCode, IsActive, CreatedBy, DateCreated, IsItemExist, StatusId")]ExpectedReceiptLine expectedReceiptLine)
        {
            expectedReceiptLine.IsChecked = false;
            expectedReceiptLine.IsActive = true;
            expectedReceiptLine.CreatedBy = CookieHelper.EmailAddress;
            expectedReceiptLine.IsItemExist = true;

            var url = "api/GoodsIn/ExpectedReceipt/Lines/Add";

            var response = await HttpClientHelper.ApiCall(url, Method.POST, expectedReceiptLine);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = expectedReceiptLine.ItemCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = expectedReceiptLine.ItemDescription + " successfully created!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode, IsActive = expectedReceiptLine.IsActive });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Create||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode, IsActive = expectedReceiptLine.IsActive });

            }
            return PartialView(expectedReceiptLine);

            //using (var client = new HttpClient())
            //{
            //    var url = new Uri(ConfigHelper.BaseUrl) + "api/GoodsIn/ExpectedReceipt/Lines/Add";
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CookieHelper.Token);
            //    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, expectedReceiptLine);

            //    if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
            //    {
            //        TempData["Message"] = expectedReceiptLine.ItemCode + " is already exist! Please check and try again.";
            //        TempData["MessageAlert"] = "warning";
            //        return RedirectToAction("Index");
            //    }

            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        TempData["Message"] = "Line Successfully Created";
            //        TempData["MessageAlert"] = "success";
            //        return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber , StatusCode = expectedReceiptLine.StatusCode });
            //    }
            //    else
            //    {
            //        Error("An error has occurred");
            //        Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Create||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, responseMessage));
            //        return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            //    }
            //}

            //return PartialView(expectedReceiptLine);
        }

        public async Task<ActionResult> Details(int id, string GoodsReceivedNumber, string StatusCode)
        {

             var expectedReceiptLine = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            var url = "api/GoodsIn/ExpectedReceipt/Lines/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptId = data.ExpectedReceiptId;
                obj.Line = data.Line;
                obj.Batch = data.Batch;
                obj.Quantity = data.Quantity;
                obj.BrandName = data.Brand ==  null ? "" : data.Brand.Name;
                obj.BrandCode = data.BrandCode;
                obj.ExpiryDate = data.ExpiryDate;
                obj.ItemCode = data.ItemCode;
                obj.ItemDescription = data.ItemDescription;
                obj.ProductCode = data.Product == null ? "" : data.Product.Description;
                obj.UomDescription = data.Uom == null ? "" : data.Uom.Description;
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Details||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }

            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;
            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(int id, string StatusCode, string GoodsReceivedNumber)
        {
            var expectedReceiptLine = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            var url = "api/GoodsIn/ExpectedReceipt/Lines/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptId = data.ExpectedReceiptId;
                obj.CustomerId = data.CustomerId;
                obj.ItemId = data.ItemId;
                obj.Line = data.Line;
                obj.Batch = data.Batch;
                obj.Quantity = data.Quantity;
                obj.BrandId = data.BrandId;
                obj.BrandName = data.Brand == null ? "" : data.Brand.Name;
                obj.BrandCode = data.BrandCode;
                obj.ItemCode = data.ItemCode;
                obj.ProductId = data.ProductId;
                obj.UomId = data.UomId;
                obj.ItemDescription = data.ItemDescription;
                obj.ExpiryDate = data.ExpiryDate;
                obj.ProductCode = data.Product == null ? "" : data.Product.ProductCode;
                obj.UomDescription = data.Uom == null ? "" : data.Uom.Description;
                obj.IsActive = data.IsActive;
                obj.DateCreated = data.DateCreated;
                obj.CreatedBy = data.CreatedBy;
                obj.IsItemExist = data.IsItemExist;
                obj.StatusId = data.StatusId;

                ViewBag.ProdDesc = data.Product == null ? "" : data.Product.Description;
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Edit||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode, IsActive = expectedReceiptLine.IsActive });
            }

            
            ViewBag.ExpectedReceiptLineId = id;
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;
            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, ExpectedReceiptId, StatusId, ProductId, CustomerId, UomId, ItemId, Line, ProductCode, ItemDescription, ItemCode, Batch, ExpiryDate, Quantity, UomDescription, BrandId, BrandCode, BrandName, IsChecked, GoodsReceivedNumber, UpdatedBy, StatusCode, IsActive, CreatedBy, DateCreated, DateUpdated, IsItemExist")]
        ExpectedReceiptViewModel.ExpectedReceiptLine expectedReceiptLine)
        {
            expectedReceiptLine.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/GoodsIn/ExpectedReceipt/Lines/Update";
            dynamic obj = new
            {
                expectedReceiptLine.Id,
                expectedReceiptLine.CustomerId,
                expectedReceiptLine.ExpectedReceiptId,
                expectedReceiptLine.ProductId,
                expectedReceiptLine.UomId,
                expectedReceiptLine.ItemId,
                expectedReceiptLine.BrandId,
                expectedReceiptLine.BrandName,
                expectedReceiptLine.Line,
                expectedReceiptLine.ProductCode,
                expectedReceiptLine.ItemDescription,
                expectedReceiptLine.ItemCode,
                expectedReceiptLine.BrandCode,
                expectedReceiptLine.Batch,
                expectedReceiptLine.ExpiryDate,
                expectedReceiptLine.Quantity,
                expectedReceiptLine.UomDescription,
                expectedReceiptLine.IsActive,
                expectedReceiptLine.DateCreated,
                expectedReceiptLine.CreatedBy,
                expectedReceiptLine.DateUpdated,
                expectedReceiptLine.UpdatedBy,
                expectedReceiptLine.IsItemExist,
                expectedReceiptLine.StatusId
            };

            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = expectedReceiptLine.ItemCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = expectedReceiptLine.ItemDescription + " successfully updated!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Edit||DeliveryRequest ID::{0}||API Response::{1}", obj.Id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            return PartialView(expectedReceiptLine);

        }

        public async Task<ActionResult> Delete(int? id, int expectedReceiptId, string GoodsReceivedNumber, string StatusCode)
        {
            var expectedReceiptLine = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            var url = "api/GoodsIn/ExpectedReceipt/Lines/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptId = data.ExpectedReceiptId;
                obj.Line = data.Line;
                obj.CustomerId = data.CustomerId;
                obj.Batch = data.Batch;
                obj.Quantity = data.Quantity;
                obj.BrandName = data.Brand == null ? "" : data.Brand.Name;
                obj.BrandCode = data.BrandCode;
                obj.ExpiryDate = data.ExpiryDate;
                obj.ItemCode = data.ItemCode;
                obj.ItemDescription = data.ItemDescription;
                obj.ProductCode = data.Product == null ? "" : data.Product.Description;
                obj.UomDescription = data.Uom == null ? "" : data.Uom.Description;
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Delete||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, int expectedReceiptId, ExpectedReceiptViewModel.ExpectedReceiptLine expectedReceiptLine)
        {
            var url = $"api/GoodsIn/ExpectedReceipt/Lines/Delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Line successfully deleted!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Delete||Expected Receipt Lines ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            //using (var client = new HttpClient())
            //{
            //    var url = new Uri(ConfigHelper.BaseUrl) + "api/GoodsIn/ExpectedReceipt/Lines/Delete";
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CookieHelper.Token);
            //    HttpResponseMessage responseMessage = await client.DeleteAsync(url + "/" + id);

            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        TempData["Message"] = "Line Successfully Deleted";
            //        TempData["MessageAlert"] = "success";
            //        return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            //    }
            //    else
            //    {
            //        Error("An error has occurred");
            //        Log.Error(string.Format(Type.GetType(typeof().Name) + "||Delete||Expected Receipt Lines ID::{0}||API Response::{1}", id, responseMessage));
            //        return RedirectToAction("Index", "ExpectedReceiptLines", new { id = eExpectedReceiptLinesControllerxpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            //    }
            //}
        }

        public async Task<ActionResult> Enable(int? id, int expectedReceiptId, string GoodsReceivedNumber, string StatusCode)
        {
            var expectedReceiptLine = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            var url = "api/GoodsIn/ExpectedReceipt/Lines/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptId = data.ExpectedReceiptId;
                obj.Line = data.Line;
                obj.CustomerId = data.CustomerId;
                obj.Batch = data.Batch;
                obj.Quantity = data.Quantity;
                obj.BrandName = data.Brand == null ? "" : data.Brand.Name;
                obj.BrandCode = data.BrandCode;
                obj.ExpiryDate = data.ExpiryDate;
                obj.ItemCode = data.ItemCode;
                obj.ItemDescription = data.ItemDescription;
                obj.ProductCode = data.Product == null ? "" : data.Product.Description;
                obj.UomDescription = data.Uom == null ? "" : data.Uom.Description;
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Delete||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id, int expectedReceiptId, ExpectedReceiptViewModel.ExpectedReceiptLine expectedReceiptLine)
        {
            var url = $"api/GoodsIn/ExpectedReceipt/Lines/Enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Line successfully enabled!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Enable||Expected Receipt Lines ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index", "ExpectedReceiptLines", new { id = expectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetDefaultBrandJson(int productId)
        {
            var expectedReceiptLine = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            if (productId == null)
            {
                return Json (new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceiptLine();
            var url = "api/Brand/GetByProductId/" + productId;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptId = data.ExpectedReceiptId;
                obj.Line = data.Line;
                obj.Batch = data.Batch;
                obj.Quantity = data.Quantity;
                obj.BrandName = data.BrandName;
                obj.BrandCode = data.BrandCode;
                obj.ExpiryDate = data.ExpiryDate;
                obj.ItemCode = data.ItemCode;
                obj.ItemDescription = data.ItemDescription;
                obj.ProductCode = data.Product == null ? "" : data.Product.Description;
                obj.UomDescription = data.Uom == null ? "" : data.Uom.Description;
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptLinesController).Name) + "||Details||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return Json("Index", "ExpectedReceiptLines");
            }

            return Json(obj);
        }
    }
}