    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Web.Models.ViewModels;
using System.Threading.Tasks;
using Application.Web.Helper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using RestSharp;
using Application.Web.Models;

namespace Application.Web.Controllers.Transaction
{
    public class ExpectedReceiptController : BaseController
    {
        private static readonly long ForReceivingStatus = ConfigHelper.ForReceivingStatus;
        private static readonly long CompletedStatus = ConfigHelper.CompletedStatus;
        private static readonly long ReceivedStatus = ConfigHelper.ReceivedStatus;
        private int _customerId = int.Parse(CookieHelper.CustomerId);
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: ExpectedReceipt
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var dateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dateTo = DateTime.Today;
          
       
            ViewBag.DateFrom = dateFrom.ToString("MM/dd/yyyy");
            ViewBag.DateTo = dateTo.ToString("MM/dd/yyyy");

            return View();
        }

        
        [HttpPost]
        public async Task<JsonResult> IndexJson(ExpectedReceiptFilterViewModels request, bool? Planned, string StatusCode)
        {
            var helper = new GoodsInDataTableSearchHelper();
            var result = await helper.SearchFunction(request, true);

            var resultList = (IEnumerable<ExpectedReceiptViewModel.ExpectedReceipt>)result.filteredList;

            var data = resultList.Select(x => new
            {

                Actions = "<div class='btn-group' style='display:flex;'>" +
                            "<a class='btn btn-primary' href='#'><i class='fa fa-gear fa-fw'></i> Action</a>" +
                            "<a class='btn btn-primary dropdown-toggle' data-toggle='dropdown' href='#'>" +
                                "<span class='fa fa-caret-down' title='Toggle dropdown menu'></span>" +
                            "</a>" +
                            "<ul class='dropdown-menu' role='menu'>" +
                            (x.StatusCode.Equals("For Receiving", StringComparison.OrdinalIgnoreCase) ?
                            (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" : "") +
                              "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                             (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Delete</a></li>" :
                                              "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>") : "") + 

                                 (x.StatusCode.Equals("Received",StringComparison.OrdinalIgnoreCase) ?
                                 "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                                 "<li class='complete' data-tranid='" + x.Id + "' href='javascript:void(0)' onclick='btnCompleted($(this))'><a>Complete</a></li>" : "" ) +

                                 (x.StatusCode.Equals("Completed", StringComparison.OrdinalIgnoreCase) ?
                                 "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" : "") +
                            "</ul>" +
                          "</div>",

                ExpectedReceiptDate = x.ExpectedReceiptDate.ToString("MMM. dd, yyyy"),
                GoodsReceivedNumber = x.IsActive ? "<a href='" + Url.Action("Index", "ExpectedReceiptLines", new { x.Id, x.GoodsReceivedNumber, x.StatusCode, x.IsActive }) + "'>" + x.GoodsReceivedNumber + "</a>" : "<a href='" + Url.Action("Index", "ExpectedReceiptLines", new { x.Id, x.GoodsReceivedNumber, x.StatusCode, x.IsActive }) + "'>" + x.GoodsReceivedNumber + "</a>" ,
                AutoReferenceNumber = x.AutoReferenceNumber,
                ReferenceNumber = x.ReferenceNumber,
                ReceivedDate = x.ReceivedDate?.ToString("MMM. dd, yyyy") ?? "",
                ReceivedBy = x.ReceivedBy,
                StatusCode = x.StatusCode,
                WarehouseCode = x.WarehouseCode,
                Address = x.Address,
                Supplier = x.Supplier,
                HaulierName = x.HaulierName,
                Comments = x.Comments,
                IsActive = x.IsActive,

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
            var obj = new ExpectedReceiptViewModel.ExpectedReceipt();
            var url = $"api/GoodsIn/ExpectedReceipt/GenerateRN/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET, url);

            if(response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.CustomerId = int.Parse(CookieHelper.CustomerId);
                obj.ReferenceNumber = data;
            };

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, GoodsReceivedNumber, AutoReferenceNumber, ReferenceNumber, ExpectedReceiptDate, WarehouseId, WarehouseCode," +
            " Address, Supplier, VendorId, HaulierId, HaulierName, HaulierCode, Comments, Received, ReceivedDate, DateCreated, StatusCode, StatusId, ReceivedBy, Planned, IsActive, CreatedBy")]
        ExpectedReceiptViewModel.ExpectedReceipt expectedReceipt)
        {
            expectedReceipt.IsProcessing = false;
            expectedReceipt.Planned = true;
            expectedReceipt.IsActive = true;
            expectedReceipt.StatusId = ForReceivingStatus;
            expectedReceipt.CreatedBy = CookieHelper.EmailAddress;

            var url = "api/GoodsIn/ExpectedReceipt/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, expectedReceipt);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = expectedReceipt.ReferenceNumber + " is already exist! Please check your Reference Number and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index", new { StatusId = expectedReceipt.StatusId });
            }

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                TempData["Message"] = expectedReceipt.ReferenceNumber + " successfully created!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceipt", new { StatusId = expectedReceipt.StatusId });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptController).Name) + "||Create||Expected Receipt ID::{0}||API Response::{1}", expectedReceipt.Id, response));
                return RedirectToAction("Index", "ExpectedReceipt", new { StatusId = expectedReceipt.StatusId });
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceipt();
            var url = "api/GoodsIn/ExpectedReceipt/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptDate = data.ExpectedReceiptDate;
                obj.AutoReferenceNumber = data.AutoReferenceNumber;
                obj.GoodsReceivedNumber = data.GoodsReceivedNumber;
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.StatusId = data.StatusId;
                obj.Address = data.Address;
                obj.WarehouseCode = data.Warehouse.Description;
                obj.ReceivedBy = data.ReceivedBy;
                obj.ReceivedDate = data.ReceivedDate;
                obj.Supplier = data.Supplier;
                obj.HaulierName = data.Haulier == null ? "" : data.Haulier.Name;
                obj.HaulierCode = data.Haulier == null ? "" : data.Haulier.HaulierCode;
                obj.StatusCode = data.Status == null ? "" : data.Status.Name;
                obj.Planned = data.Planned;
                obj.IsActive = data.IsActive;
            }

            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceipt();
            var url = "api/GoodsIn/ExpectedReceipt/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.CustomerId = data.CustomerId;
                obj.ExpectedReceiptDate = data.ExpectedReceiptDate;
                obj.DateCreated = data.DateCreated;
                obj.GoodsReceivedNumber = data.GoodsReceivedNumber;
                obj.AutoReferenceNumber = data.AutoReferenceNumber;
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.Address = data.Address;
                obj.WarehouseDescription = data.Warehouse.Description;
                obj.WarehouseCode = data.Warehouse.WarehouseCode;
                obj.ReceivedBy = data.ReceivedBy;
                obj.ReceivedDate = data.ReceivedDate;
                obj.VendorId = data.VendorId;
                obj.Supplier = data.Vendor != null ? data.Vendor.VendorName : data.Supplier;
                obj.StatusId = data.StatusId;
                obj.HaulierId = data.HaulierId;
                obj.HaulierName = data.Haulier == null ? "" : data.Haulier.Name;
                obj.HaulierCode = data.Haulier == null ? "" : data.Haulier.HaulierCode;
                obj.StatusCode = data.Status == null ? "" : data.Status.Name;
                obj.Planned = data.Planned;
                obj.IsActive = data.IsActive;
                obj.DateCreated = data.DateCreated;
                obj.CreatedBy = data.CreatedBy;
                obj.IsProcessing = data.IsProcessing;
            }

            ViewBag.EmailAddress = CookieHelper.EmailAddress;
            ViewBag.StatCode = obj.StatusCode;
            return PartialView(obj);

        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, GoodsReceivedNumber, AutoReferenceNumber, IsProcessing, ReferenceNumber, ExpectedReceiptDate, WarehouseId, WarehouseCode, Address," +
            "VendorId, Supplier, HaulierId, HaulierName, HaulierCode, Comments, Received, ReceivedDate, StatusId, DateCreated, StatusCode, ReceivedBy, Planned," +
            "UpdatedBy, IsActive, DateCreated, CreatedBy, IsProcessing")]ExpectedReceiptViewModel.ExpectedReceipt expectedReceipt)
        {
            //expectedReceipt.CustomerId = 1;
            //expectedReceipt.Planned = true;
            expectedReceipt.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/GoodsIn/ExpectedReceipt/Update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, expectedReceipt);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = expectedReceipt.ReferenceNumber + " is already exist! Please check your Reference Number and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index", "ExpectedReceipt", new { StatusId = expectedReceipt.StatusId });
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = expectedReceipt.ReferenceNumber + " is successfully updated!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceipt", new { StatusId = expectedReceipt.StatusId });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptController).Name) + "||Update||Expected Receipt ID::{0}||API Response::{1}", expectedReceipt.Id, response));
                return RedirectToAction("Index", "ExpectedReceipt", new { StatusId = expectedReceipt.StatusId });
            }
            return PartialView(expectedReceipt);
        }

        public async Task<ActionResult> Delete(int? id, string StatusCode)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceipt();
            var url = "api/GoodsIn/ExpectedReceipt/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptDate = data.ExpectedReceiptDate;
                obj.AutoReferenceNumber = data.AutoReferenceNumber;
                obj.GoodsReceivedNumber = data.GoodsReceivedNumber;
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.Address = data.Address;
                obj.WarehouseCode = data.Warehouse.Description;
                obj.ReceivedBy = data.ReceivedBy;
                obj.ReceivedDate = data.ReceivedDate;
                obj.Supplier = data.Supplier;
                obj.HaulierName = data.Haulier == null ? "" : data.Haulier.Name;
                obj.HaulierCode = data.Haulier == null ? "" : data.Haulier.HaulierCode;
                obj.StatusCode = data.Status == null ? "" : data.Status.Name;
                obj.Planned = data.Planned;
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {

            var url = $"api/GoodsIn/ExpectedReceipt/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Header successfully deleted!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceipt");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptController).Name) + "||Delete||Expected Receipt ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Enable(int? id, string StatusCode)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new ExpectedReceiptViewModel.ExpectedReceipt();
            var url = "api/GoodsIn/ExpectedReceipt/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptDate = data.ExpectedReceiptDate;
                obj.AutoReferenceNumber = data.AutoReferenceNumber;
                obj.GoodsReceivedNumber = data.GoodsReceivedNumber;
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.Address = data.Address;
                obj.WarehouseCode = data.Warehouse.Description;
                obj.ReceivedBy = data.ReceivedBy;
                obj.ReceivedDate = data.ReceivedDate;
                obj.Supplier = data.Supplier;
                obj.HaulierName = data.Haulier == null ? "" : data.Haulier.Name;
                obj.HaulierCode = data.Haulier == null ? "" : data.Haulier.HaulierCode;
                obj.StatusCode = data.Status == null ? "" : data.Status.Name;
                obj.Planned = data.Planned;
                obj.UpdatedBy = CookieHelper.EmailAddress;
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id)
        {

            var url = $"api/GoodsIn/ExpectedReceipt/Enable/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Header successfully enabled!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "ExpectedReceipt");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptController).Name) + "||Enable||Expected Receipt ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<JsonResult> Complete(int id)
        {
            var url = "";
            if (id == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }

            var obj = new ExpectedReceiptViewModel.ExpectedReceipt();
            url = "api/GoodsIn/ExpectedReceipt/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            obj.CustomerId = 1;

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.ExpectedReceiptDate = data.ExpectedReceiptDate;
                obj.AutoReferenceNumber = data.AutoReferenceNumber;
                obj.GoodsReceivedNumber = data.GoodsReceivedNumber;
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.Address = data.Address;
                obj.WarehouseDescription = data.Warehouse.Description;
                obj.WarehouseCode = data.Warehouse.WarehouseCode;
                obj.ReceivedBy = data.ReceivedBy;
                obj.ReceivedDate = data.ReceivedDate;
                obj.Supplier = data.Supplier;
                obj.StatusId = CompletedStatus;
                obj.HaulierId = data.HaulierId;
                obj.HaulierName = data.Haulier == null ? "" : data.Haulier.Name;
                obj.HaulierCode = data.Haulier == null ? "" : data.Haulier.HaulierCode;
                obj.Planned = data.Planned;
                obj.IsActive = data.IsActive;
                obj.DateCreated = data.DateCreated;
                obj.CreatedBy = data.CreatedBy;
                obj.UpdatedBy = CookieHelper.EmailAddress;
            }

            var urlLine = "api/GoodsIn/ExpectedReceipt/Lines/GetList/" + id;
            var responseLine = await HttpClientHelper.ApiCall(urlLine, Method.GET);
            if (responseLine.Content == "[]")
            {
                TempData["Message"] = obj.GoodsReceivedNumber + " has no line item! Unable to mark it as COMPLETED!";
                TempData["MessageAlert"] = "warning";
                return Json("Index", "ExpectedReceipt");
            }
            else
            {

                url = "api/GoodsIn/ExpectedReceipt/Update";
                var responseMess = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

                if (responseMess.IsSuccessful)
                {
                        TempData["Message"] = obj.GoodsReceivedNumber + " is already COMPLETED!";
                        TempData["MessageAlert"] = "success";
                        return Json("Index", "ExpectedReceipt");
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(ExpectedReceiptController).Name) + "||Update||Expected Receipt ID::{0}||API Response::{1}", id, responseMess));
                    return Json("Index", "ExpectedReceipt");
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> ValidateReferenceNumber(string referenceNumber, int customerId)
        {
            var url = "api/GoodsIn/ExpectedReceipt/CheckRefNumber/" + referenceNumber + "/" + _customerId;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Json(new { status = "success", message = "Reference Number is already taken."});
            }
            return null;
        }
    }
}