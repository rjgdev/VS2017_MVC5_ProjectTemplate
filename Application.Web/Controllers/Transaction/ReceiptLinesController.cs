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

namespace Application.Web.Controllers.Transaction
{
    public class ReceiptLinesController : BaseController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ExpectedReceiptLines
        [Authorize]
        public async Task<ActionResult> Index(int? id, string GoodsReceivedNumber, string StatusCode)
        {

            //var list = new List<ExpectedReceiptViewModel.ExpectedReceiptLine>();
            //var url = "api/GoodsIn/ExpectedReceipt/Lines/GetList/" + id;
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

            //    list = serialize.Select(x => new ExpectedReceiptViewModel.ExpectedReceiptLine
            //    {
            //        Id = x.Id,
            //        ExpectedReceiptId = x.ExpectedReceiptId,
            //        Line = x.Line,
            //        ProductId = x.ProductId,
            //        Quantity = x.Quantity,
            //        BrandCode = x.BrandCode,
            //        BrandId = x.BrandId,
            //        BrandName = x.Brand == null ? "" : x.Brand.Name,
            //        Batch = x.Batch,
            //        ItemCode = x.ItemCode,
            //        ItemDescription = x.ItemDescription,
            //        ExpiryDate = x.ExpiryDate != null ? x.Expirydate : null,
            //        //ExpiryDate = DateTime.Parse(x.ExpiryDate.ToString("MMM. dd, yyyy").ToLower()),
            //        //ExpiryDate = x.ExpiryDate.ToString("MMM. dd, yyyy").ToLower(),
            //        UomDescription = x.Uom == null ? "" : x.Uom.Description,
            //        ProductCode = x.Product == null ? "" : x.Product.Description,
            //        IsItemExist = x.IsItemExist

            //    }).ToList();
            //}
            //else
            //{
            //    Error("An error has occurred");
            //    Log.Error(string.Format(Type.GetType(typeof(ReceiptLinesController).Name) + "||Index||Receipt Lines ID::{0}||API Response::{1}", id, response));
            //    return RedirectToAction("Index", "ReceiptLines");
            //}

            ViewBag.ExpectedReceiptId = id;
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterLineViewModels request, bool? Planned, string StatusCode)
        {
            var helper = new ReceiptLineDataTableSearchHelper();
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
                            (x.StatusCode.Equals("Received", StringComparison.OrdinalIgnoreCase) && (x.IsItemExist == false) ?
                                    "<li class='crud' data-id='" + x.Id + "' data-status='" + x.StatusCode + "' data-grn='" + x.GoodsReceivedNumber + "' data-request-url='" + Url.Action("Details") +
                                    "' onclick='btnClicked($(this))'><a>Details</a></li>" : "" ) +

                                     (x.StatusCode.Equals("Received", StringComparison.OrdinalIgnoreCase) && (x.IsItemExist == true) ?
                                 "<li class='crud' data-id='" + x.Id + "' data-grn='" + x.GoodsReceivedNumber + "' data-status='" + x.StatusCode + "' data-request-url='" + Url.Action("Edit") + "' onclick='btnClicked($(this))'><a>Edit</a></li>" +
                                    "<li class='crud' data-id='" + x.Id + "' data-grn='" + x.GoodsReceivedNumber + "' data-status='" + x.StatusCode + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" : "") +

                                 (x.StatusCode.Equals("Completed", StringComparison.OrdinalIgnoreCase) && (x.IsItemExist == true) ?
                                 "<li class='crud' data-id='" + x.Id + "' data-request-url='" + Url.Action("Details") + "' onclick='btnClicked($(this))'><a>Details</a></li>" : "") +
                            "</ul>" +
                          "</div>",

                ExpiryDate = x.ExpiryDate?.ToString("MMM. dd, yyyy") ?? "",
                ItemCode = x.IsItemExist ? x.ItemCode : "<a class='ItemCode' data-id='" + x.Id + "' data-expreceiptid='" + x.ExpectedReceiptId + "' data-grn='" + x.GoodsReceivedNumber + "' data-status='" + x.StatusCode + "' data-item='" + x.ItemCode + "' data-quantity='" + x.Quantity + "' data-itemdesc='" + x.ItemDescription + "' data-request-url='" + Url.Action("CreateNewItem") + "' onclick = 'btnCreateNewItem($(this))' > " + x.ItemCode + "</a>",
                BrandName = x.BrandName,
                ItemDescription = x.ItemDescription,
                Quantity = x.Quantity,
                UomDescription = x.UomDescription
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
                obj.BrandName = data.Brand == null ? "" : data.Brand.Name;
                obj.BrandCode = data.BrandCode;
                obj.ExpiryDate = data.ExpiryDate;
                obj.ItemCode = data.ItemCode;
                obj.ItemDescription = data.ItemDescription;
                obj.IsItemExist = data.IsItemExist;
                obj.ProductCode = data.Product == null ? "" : data.Product.Description;
                obj.UomDescription = data.Uom == null ? "" : data.Uom.Description;
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ReceiptLinesController).Name) + "||Details||Expected Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", "ReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber });
            }
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            ViewBag.StatCode = StatusCode;
            return PartialView(obj);
        }

        public async Task<ActionResult> CreateNewItem(int ExpectedReceiptId, int ExpectedReceiptLineId, string GoodsReceivedNumber, string StatusCode, string ItemCode, string ItemDescription, int Quantity)
        {
            var obj = new ItemViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId),
                ItemCode = ItemCode,
                Description = ItemDescription,
                Quantity = Quantity,
                GoodsReceivedNumber = GoodsReceivedNumber,
                StatusCode = StatusCode,
                ExpectedReceiptLineId = ExpectedReceiptLineId,
            };

            //ViewBag.grn = GoodsReceivedNumber;
            //ViewBag.StatCode = StatusCode;
            //ViewBag.ExpectedReceiptId = ExpectedReceiptId;

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewItem([Bind(Include = "CustomerId, ItemCode, Description, ProductCode, BrandId, WarehouseCode," +
            "LocationId, Quantity, GoodsReceivedNumber, StatusCode, ExpectedReceiptId, ExpectedReceiptLineId, CreatedBy, IsActive")]ItemViewModel obj)
        {
            //item.CustomerId = int.Parse(CookieHelper.CustomerId);
            obj.CreatedBy = CookieHelper.EmailAddress;
            obj.IsActive = true;

            var url = "api/GoodsIn/ExpectedReceipt/Lines/AddLineItem/" + obj.ExpectedReceiptLineId;

            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.ItemCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Item successfully created!";
                TempData["MessageAlert"] = "success";
                return RedirectToAction("Index", new { id = obj.ExpectedReceiptId, GoodsReceivedNumber = obj.GoodsReceivedNumber, StatusCode = obj.StatusCode });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ItemController).Name) + "||Create||Item ID::{0}||API Response::{1}", obj.Id, response));
                return RedirectToAction("Index");
            }
            //return PartialView();
            return RedirectToAction("Index", "ReceipLines");
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
                obj.CustomerId = data.CustomerId;
                obj.IsItemExist = data.IsItemExist;
                obj.IsActive = data.IsActive;
                obj.IsChecked = data.IsChecked;
                obj.ExpectedReceiptId = data.ExpectedReceiptId;
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
                obj.UpdatedBy = data.UpdatedBy;

                ViewBag.ProdDesc = data.Product == null ? "" : data.Product.Description;

            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ReceiptLinesController).Name) + "||Edit||Receipt Lines ID::{0}||API Response::{1}", expectedReceiptLine.Id, response));
                return RedirectToAction("Index", "ReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber});
            }
            ViewBag.ExpectedReceiptLineId = id;
            ViewBag.StatCode = StatusCode;
            ViewBag.GoodsReceivedNo = GoodsReceivedNumber;
            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, ExpectedReceiptId, CustomerId, ProductId, UomId, ItemId, Line, ProductCode, ItemDescription, ItemCode, Brand, Batch, " +
            "ExpiryDate, Quantity, UomDescription, BrandId, BrandCode, BrandName, IsItemExist, IsActive, IsChecked, GoodsReceivedNumber, StatusCode, UpdatedBy")]ExpectedReceiptViewModel.ExpectedReceiptLine expectedReceiptLine)
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
                expectedReceiptLine.IsChecked,
                expectedReceiptLine.IsActive,
                expectedReceiptLine.IsItemExist,
                expectedReceiptLine.Line,
                expectedReceiptLine.BrandId,
                expectedReceiptLine.BrandName,
                expectedReceiptLine.ProductCode,
                expectedReceiptLine.ItemDescription,
                expectedReceiptLine.ItemCode,
                expectedReceiptLine.BrandCode,
                expectedReceiptLine.Batch,
                expectedReceiptLine.ExpiryDate,
                expectedReceiptLine.Quantity,
                expectedReceiptLine.UomDescription,
                expectedReceiptLine.UpdatedBy

            };

            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Line successfully updated!";
                return RedirectToAction("Index", "ReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ReceiptLinesController).Name) + "||Edit||DeliveryRequest ID::{0}||API Response::{1}", obj.Id, response));
                return RedirectToAction("Index", "ReceiptLines", new { id = expectedReceiptLine.ExpectedReceiptId, GoodsReceivedNumber = expectedReceiptLine.GoodsReceivedNumber, StatusCode = expectedReceiptLine.StatusCode });
            }
            return PartialView(expectedReceiptLine);

        }
    }
}