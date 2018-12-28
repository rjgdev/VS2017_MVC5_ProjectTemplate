using Application.Web.Helper;
using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Application.Web.Controllers
{
    public class ReceiptController : BaseController
    {
        // GET: ReceiptTransaction
        public async Task<ActionResult> Index()
        {
            //var list = new List<ExpectedReceiptViewModel.ExpectedReceipt>();
            //var url = "api/GoodsIn/ExpectedReceipt/GetUnPlanned/" + true + "/" + int.Parse(CookieHelper.CustomerId);
            //var response = await HttpClientHelper.ApiCall(url, Method.GET);

            //if (response.IsSuccessful)
            //{
            //    var result = response.Content;

            //    var settings = new JsonSerializerSettings
            //    {
            //        NullValueHandling = NullValueHandling.Ignore,
            //        MissingMemberHandling = MissingMemberHandling.Ignore
            //    };
            //    var serialize = JsonConvert.DeserializeObject<List<dynamic>>(result, settings);

            //    list = serialize.Select(x => new ExpectedReceiptViewModel.ExpectedReceipt
            //    {
            //        Id = x.Id,
            //        ExpectedReceiptDate = x.ExpectedReceiptDate,
            //        DateCreated = x.DateCreated,
            //        GoodsReceivedNumber = x.GoodsReceivedNumber,
            //        ReferenceNumber = x.ReferenceNumber,
            //        Received = x.Received,
            //        Comments = x.Comments,
            //        Address = x.Address,
            //        WarehouseCode = x.Warehouse.Description,
            //        ReceivedBy = x.ReceivedBy,
            //        ReceivedDate = x.ReceivedDate,
            //        Supplier = x.Supplier,
            //        HaulierName = x.Haulier == null ? "" : x.Haulier.Name,
            //        HaulierCode = x.Haulier == null ? "" : x.Haulier.HaulierCode,
            //        StatusCode = x.Status == null ? "" : x.Status.Name,
            //        Planned = x.Planned


            //    }).ToList();

            //    //var isPlannedSelectList = new List<SelectListItem>
            //    //{
            //    //    new SelectListItem { Value = "False", Text = "Unplanned"},
            //    //    new SelectListItem { Value = "True", Text = "Planned"}
            //    //};

            //    //ViewBag.Planned = isPlannedSelectList;
            //    var statusSelectList = new List<SelectListItem>
            //    {
            //        new SelectListItem { Value = "Received", Text = "Received"},
            //        new SelectListItem { Value = "Completed", Text = "Completed"}
            //    };

            //    ViewBag.StatusCode = statusSelectList;
            //}

                return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ExpectedReceiptFilterViewModels request, bool? Planned, string StatusCode)
        {
            var helper = new GoodsInDataTableSearchHelper();
            var result = await helper.SearchFunction(request, false);

            var resultList = (IEnumerable<ExpectedReceiptViewModel.ExpectedReceipt>)result.filteredList;

            var data = resultList.Select(x => new
            {

                Actions = "<div class='btn-group' style='display:flex;'>" +
                            "<a class='btn btn-primary' href='#'><i class='fa fa-gear fa-fw'></i> Action</a>" +
                            "<a class='btn btn-primary dropdown-toggle' data-toggle='dropdown' href='#'>" +
                                "<span class='fa fa-caret-down' title='Toggle dropdown menu'></span>" +
                            "</a>" +
                            "<ul class='dropdown-menu' role='menu'>" +
                            //(x.StatusCode.Equals("For Receiving", StringComparison.OrdinalIgnoreCase) ?
                            // "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" +
                            //  "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                            // (x.IsActive ? "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Delete") + "' onclick='btnClicked($(this))'><a>Disable</a></li>" :
                            //                  "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Enable") + "' onclick='btnClicked($(this))'><a>Enable</a></li>") : "") +

                                 (x.StatusCode.Equals("Received", StringComparison.OrdinalIgnoreCase) ?
                                    "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" +
                                 "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" +
                                 "<li class='complete' data-tranid='" + x.Id + "' href='javascript:void(0)' onclick='btnCompleted($(this))'><a>Complete</a></li>" : "") +

                                 (x.StatusCode.Equals("Completed", StringComparison.OrdinalIgnoreCase) ?
                                 "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" : "") +
                            "</ul>" +
                          "</div>",

                ExpectedReceiptDate = x.ExpectedReceiptDate.ToString("MMM. dd, yyyy"),
                GoodsReceivedNumber = x.IsActive ? "<a href='" + Url.Action("Index", "ReceiptLines", new { x.Id, x.GoodsReceivedNumber, x.StatusCode }) + "'>" + x.GoodsReceivedNumber + "</a>" : "<a href='" + Url.Action("Index", "ReceiptLines", new { x.Id, x.GoodsReceivedNumber, x.StatusCode }) + "'>" + x.GoodsReceivedNumber + "</a>",
                ReferenceNumber = x.ReferenceNumber,
                ReceivedDate = x.ReceivedDate?.ToString("MMM. dd, yyyy") ?? "",
                ReceivedBy = x.ReceivedBy,
                StatusCode = x.StatusCode,
                WarehouseCode = x.WarehouseCode,
                Address = x.Address,
                Supplier = x.Supplier,
                HaulierName = x.HaulierName,
                Comments = x.Comments

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

        //[HttpPost, ActionName("Index")]
        //public async Task<ActionResult> IndexPost(string StatusCode)
        //{
        //    string url = "";
        //    if (StatusCode == "")
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    if (StatusCode.Equals("Received"))
        //    {
        //        url = "api/GoodsIn/ExpectedReceipt/GetReceived/" + true + "/" + int.Parse(CookieHelper.CustomerId);
        //    }
        //    if (StatusCode.Equals("Completed"))
        //    {
        //        url = "api/GoodsIn/ExpectedReceipt/GetCompleted/" + true + "/" + int.Parse(CookieHelper.CustomerId);
        //    }

        //    var list = new List<ExpectedReceiptViewModel.ExpectedReceipt>();
        //    var response = await HttpClientHelper.ApiCall(url, Method.GET);

        //    if (response.IsSuccessful)
        //    {
        //        var result = response.Content;

        //        var settings = new JsonSerializerSettings
        //        {
        //            NullValueHandling = NullValueHandling.Ignore,
        //            MissingMemberHandling = MissingMemberHandling.Ignore
        //        };
        //        var serialize = JsonConvert.DeserializeObject<List<dynamic>>(result, settings);

        //        list = serialize.Select(x => new ExpectedReceiptViewModel.ExpectedReceipt
        //        {
        //            Id = x.Id,
        //            ExpectedReceiptDate = x.ExpectedReceiptDate,
        //            GoodsReceivedNumber = x.GoodsReceivedNumber,
        //            ReferenceNumber = x.ReferenceNumber,
        //            Received = x.Received,
        //            Comments = x.Comments,
        //            Address = x.Address,
        //            WarehouseCode = x.Warehouse.Description,
        //            ReceivedBy = x.ReceivedBy,
        //            ReceivedDate = x.ReceivedDate,
        //            Supplier = x.Supplier,
        //            HaulierName = x.Haulier == null ? "" : x.Haulier.Name,
        //            HaulierCode = x.Haulier == null ? "" : x.Haulier.HaulierCode,
        //            StatusCode = x.Status == null ? "" : x.Status.Name,
        //            Planned = x.Planned

        //        }).ToList();

        //        var getUnplanned = list.Where(x => x.Planned == false).ToList();

        //        list = getUnplanned;

        //        var statusSelectList = new List<SelectListItem>
        //        {
        //            new SelectListItem { Value = "Received", Text = "Received"},
        //            new SelectListItem { Value = "Completed", Text = "Completed"}
        //        };

        //        ViewBag.StatusCode = statusSelectList;
        //    }
        //    return View(list);
        //}

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
                obj.GoodsReceivedNumber = data.GoodsReceivedNumber;
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.Address = data.Warehouse != null ? data.Warehouse.Address1 : "";
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

        public async Task<ActionResult> Edit(int id)
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
                obj.ReferenceNumber = data.ReferenceNumber;
                obj.Received = data.Received;
                obj.Comments = data.Comments;
                obj.Address = data.Warehouse != null ? data.Warehouse.Address1 : "";
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
                obj.DateCreated = data.DateCreated;
                obj.CreatedBy = data.CreatedBy;
                obj.IsActive = data.IsActive;
                obj.Received = data.Received;
            }

            ViewBag.EmailAddress = CookieHelper.EmailAddress;
            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, GoodsReceivedNumber, ReferenceNumber, ExpectedReceiptDate, WarehouseId, VendorId, WarehouseCode, Address, Supplier, HaulierId," +
            "HaulierName, HaulierCode, Comments, Received, ReceivedDate, DateCreated, StatusId, StatusCode, ReceivedBy, Planned, UpdatedBy, IsActive, DateCreated, CreatedBy, DateUpdated, UpdatedBy, Received")]ExpectedReceiptViewModel.ExpectedReceipt expectedReceipt)
        {
            expectedReceipt.CustomerId = 1;
            expectedReceipt.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/GoodsIn/ExpectedReceipt/Update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, expectedReceipt);

            if (response.IsSuccessful)
            {
                TempData["Message"] = expectedReceipt.GoodsReceivedNumber + " is successfully updated!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", "Receipt", expectedReceipt.StatusCode);
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ReceiptController).Name) + "||Update||Receipt ID::{0}||API Response::{1}", expectedReceipt.Id, response));
                return RedirectToAction("Index", "Receipt");
            }
            return PartialView(expectedReceipt);

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
                obj.StatusId = 7;
                obj.HaulierId = data.HaulierId;
                obj.HaulierName = data.Haulier == null ? "" : data.Haulier.Name;
                obj.HaulierCode = data.Haulier == null ? "" : data.Haulier.HaulierCode;
                obj.Planned = data.Planned;
                obj.IsActive = data.IsActive;
                obj.DateCreated = data.DateCreated;
                obj.CreatedBy = data.CreatedBy;
                obj.UpdatedBy = CookieHelper.EmailAddress;
            }

            var list = new List<ExpectedReceiptViewModel.ExpectedReceiptLine>();
            var urlLine = "api/GoodsIn/ExpectedReceipt/Lines/GetList/" + id;
            var responseLine = await HttpClientHelper.ApiCall(urlLine, Method.GET);
            if (responseLine.Content == "[]")
            {
                TempData["Message"] = obj.GoodsReceivedNumber + " has no line item! Unable to mark it as COMPLETED!";
                TempData["MessageAlert"] = "warning";
                return Json("Index", "Receipt");
            }
            else
            {

                url = "api/GoodsIn/ExpectedReceipt/Update";
                var responseMess = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

                if (responseMess.IsSuccessful)
                {
                    TempData["Message"] = obj.GoodsReceivedNumber + " is already COMPLETED!";
                    TempData["MessageAlert"] = "success";
                    return Json("Index", "Receipt");
                }
                else
                {
                    Error("An error has occurred");
                    Log.Error(string.Format(Type.GetType(typeof(ReceiptController).Name) + "||Update||Expected Receipt ID::{0}||API Response::{1}", id, responseMess));
                    return Json("Index", "Receipt");
                }
            }
        }
    }
}