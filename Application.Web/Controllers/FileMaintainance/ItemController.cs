using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Application.Web.Helper;
using System.Net;
using RestSharp;
using System.IO;
using OfficeOpenXml;
using Application.Web.Models;

namespace Application.Web.Controllers
{
    public class ItemController : BaseController
    {
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: Item
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IndexJson(ListFilterViewModels request)
        {
            var helper = new ItemDataTableSearchHelper();
            var result = await helper.SearchFunction(request);

            var resultList = (IEnumerable<ItemViewModel>)result.filteredList;

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
                ProductDesc = x.ProductDesc,
                BrandName = x.BrandName,
                ItemCode = x.ItemCode,
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

        public ActionResult FileUpload()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            try
            {
                foreach(string file in Request.Files)
                {
                    HttpPostedFileBase fileContent = Request.Files[file];

                    Stream stream = fileContent.InputStream;
                    string fileName = Path.GetFileName(file);

                    byte[] fileBytes = new byte[fileContent.ContentLength];
                    var data = stream.Read(fileBytes, 0, Convert.ToInt32(fileContent.ContentLength));

                    using (var xls = new ExcelPackage(fileContent.InputStream))
                    {
                        var xlsSheets = xls.Workbook.Worksheets;
                        var workSheet = xlsSheets.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        var rowItr = 2;

                        var products = new List<ProductViewModel>();
                        var brands = new List<BrandViewModel>();
                        var whses = new List<WarehouseViewModel>();
                        var locs = new List<LocationViewModel>();
                        var items = new List<ItemViewModel>();

                        var method = Method.POST;
                        var url = "api/Product/GetSelectList/";
                        var response = await HttpClientHelper.ApiCall(url, Method.GET);
                        //var awaitResponse = response.GetAwaiter().GetResult();

                        if (response.IsSuccessful)
                        {
                            //var products = workSheet.Cells[rowItr, 3, noOfRow, 3].Where(x => x.Value != null).Select(x => x.Value.ToString()).Distinct();
                            products = JsonConvert.DeserializeObject<List<dynamic>>(response.Content)
                                                        .Select(x => new ProductViewModel
                                                        {
                                                            Id = x.Id,
                                                            ProductCode = x.ProductCode,
                                                            Description = x.Description
                                                        }).ToList();
                        }

                        url = "api/Brand/GetSelectList/";
                        response = await HttpClientHelper.ApiCall(url, Method.GET);
                        //awaitResponse = response.GetAwaiter().GetResult();

                        if (response.IsSuccessful)
                        {
                            //var brands = workSheet.Cells[rowItr, 4, noOfRow, 4].Where(x => x.Value != null).Select(x => x.Value.ToString()).Distinct();
                            brands = JsonConvert.DeserializeObject<List<dynamic>>(response.Content)
                                                        .Select(x => new BrandViewModel
                                                        {
                                                            Id = x.Id,
                                                            Code = x.Code,
                                                            Name = x.Name
                                                        }).ToList();
                        }

                        url = "api/Warehouse/GetSelectList/";
                        response = await HttpClientHelper.ApiCall(url, Method.GET);
                        //awaitResponse = response.GetAwaiter().GetResult();

                        if (response.IsSuccessful)
                        {
                            //var warehouses = workSheet.Cells[rowItr, 5, noOfRow, 5].Where(x => x.Value != null).Select(x => x.Value.ToString()).Distinct();
                            whses = JsonConvert.DeserializeObject<List<dynamic>>(response.Content)
                                                        .Select(x => new WarehouseViewModel
                                                        {
                                                            Id = x.Id,
                                                            WarehouseCode = x.WarehouseCode,
                                                            Description = x.Description
                                                        }).ToList();
                        }

                        url = "api/Location/GetAll/";
                        response = await HttpClientHelper.ApiCall(url, Method.GET);
                        //awaitResponse = response.GetAwaiter().GetResult();

                        if (response.IsSuccessful)
                        {
                            //var locations = workSheet.Cells[rowItr, 6, noOfRow, 6].Where(x => x.Value != null).Select(x => x.Value.ToString()).Distinct();
                            locs = JsonConvert.DeserializeObject<List<dynamic>>(response.Content)
                                                        .Select(x => new LocationViewModel
                                                        {
                                                            Id = x.Id,
                                                            LocationCode = x.LocationCode,
                                                            Description = x.Description
                                                        }).ToList();
                        }

                        url = "api/Item/GetAll/";
                        response = await HttpClientHelper.ApiCall(url, Method.GET);
                        //awaitResponse = response.GetAwaiter().GetResult();

                        if (response.IsSuccessful)
                        {
                            //var locations = workSheet.Cells[rowItr, 6, noOfRow, 6].Where(x => x.Value != null).Select(x => x.Value.ToString()).Distinct();
                            items = JsonConvert.DeserializeObject<List<dynamic>>(response.Content)
                                                        .Select(x => new ItemViewModel
                                                        {
                                                            Id = x.Id,
                                                            ItemCode = x.ItemCode,
                                                            Description = x.Description
                                                        }).ToList();
                        }

                        for (rowItr = 2; rowItr <= noOfRow; rowItr++)
                        {
                            var itemCode = workSheet.Cells[rowItr, 1].Value?.ToString() ?? "";

                            if(itemCode != null)
                            {
                                var itemDesc = workSheet.Cells[rowItr, 2].Value?.ToString() ?? "";
                                var productDesc = workSheet.Cells[rowItr, 3].Value?.ToString() ?? "";
                                var brandDesc = workSheet.Cells[rowItr, 4].Value?.ToString() ?? "";
                                var whseDesc = workSheet.Cells[rowItr, 5].Value?.ToString() ?? "";
                                var locDesc = workSheet.Cells[rowItr, 6].Value?.ToString() ?? "";
                                var quantity = long.Parse(workSheet.Cells[rowItr, 7].Value?.ToString() ?? "0");
                                var batchCode = workSheet.Cells[rowItr, 8].Value?.ToString() ?? "";

                                var product = products.FirstOrDefault(x => string.Equals(x.ProductCode, productDesc, StringComparison.OrdinalIgnoreCase)
                                                                          || string.Equals(x.Description, productDesc, StringComparison.OrdinalIgnoreCase));
                                var brand = brands.FirstOrDefault(x => string.Equals(x.Code, brandDesc, StringComparison.OrdinalIgnoreCase)
                                                                      || string.Equals(x.Name, brandDesc, StringComparison.OrdinalIgnoreCase));
                                var whse = whses.FirstOrDefault(x => string.Equals(x.WarehouseCode, whseDesc, StringComparison.OrdinalIgnoreCase)
                                                                         || string.Equals(x.Description, whseDesc, StringComparison.OrdinalIgnoreCase));
                                var loc = locs.FirstOrDefault(x => string.Equals(x.LocationCode, locDesc, StringComparison.OrdinalIgnoreCase)
                                                                       || string.Equals(x.Description, locDesc, StringComparison.OrdinalIgnoreCase));

                                var item = new ItemViewModel
                                {
                                    ItemCode = itemCode,
                                    Description = itemDesc,
                                    ProductId = product?.Id ?? null,
                                    ProductCode = product?.ProductCode ?? null,
                                    BrandId = brand?.Id ?? null,
                                    BrandCode = brand?.Code ?? null,
                                    WarehouseId = whse?.Id ?? null,
                                    WarehouseCode = whse?.WarehouseCode ?? null,
                                    LocationId = loc?.Id ?? null,
                                    LocationCode = loc?.LocationCode ?? null,
                                    CustomerId = long.Parse(CookieHelper.CustomerId),
                                    Quantity = quantity,
                                    BatchCode = batchCode,
                                    IsActive = true
                                };

                                var apiItem = items.FirstOrDefault(x => string.Equals(x.ItemCode, itemCode, StringComparison.OrdinalIgnoreCase));

                                if (apiItem == null)
                                {
                                    method = Method.POST;
                                    url = "api/Item/Add";
                                }
                                else
                                {
                                    method = Method.PUT;
                                    url = "api/Item/Update";
                                }

                                
                                response = await HttpClientHelper.ApiCall(url, method, item);
                                //var responseMessageAwait = responseMessage.GetAwaiter().GetResult();

                                if (!response.IsSuccessful)
                                {
                                    return Json(new { status = "failed", message = "An error has occurred. Not all items are uploaded."});
                                }
                                else
                                {
                                    Log.Info(string.Format(Type.GetType(typeof(ItemController).Name) + "||Upload||Item ID::{0}||API Response::{1}", item.Id, response));
                                }
                            }
                        }

                        return Json(new { status = "success", message = "File Uploading Successful." });

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Type.GetType(typeof(ItemController).Name) + "||Upload||Item||Exception:{0}", ex.Message));
                return Json(new { status = "failed", message = "An error has occurred." });
            }

            return null;
        }

        public ActionResult Create()
        {
            var obj = new ItemViewModel()
            {
                CustomerId = int.Parse(CookieHelper.CustomerId)
            };

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId, ItemCode, Description, ProductCode, BrandId")]ItemViewModel obj)
        {
            obj.CustomerId = int.Parse(CookieHelper.CustomerId);
            obj.CreatedBy = CookieHelper.EmailAddress;


            var url = "api/Item/Add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.ItemCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

            if (response.IsSuccessful)
            {
                TempData["Message"] = obj.Description + " successfully created!";
                TempData["MessageAlert"] = "success";
                //return RedirectToAction("Index", "Item");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ItemController).Name) + "||Create||Item ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index", "item");
            }
            //return PartialView();
            return RedirectToAction("Index", "item");
        }

        public async Task<ActionResult> Details(int? id)
        {
            var obj = new ItemViewModel();
            var url = "api/Item/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new ItemViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductId = data.ProductId,
                    BrandId = data.BrandId,
                    ItemCode = data.ItemCode,
                    Description = data.Description,
                    ProductCode = data.Product != null ? data.Product.ProductCode : "",
                    ProductDesc = data.Product != null ? data.Product.Description : "",
                    BrandCode = data.Brand != null ? data.Brand.Code : "",
                    BrandName = data.Brand != null ? data.Brand.Name : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var obj = new ItemViewModel();
            var url = "api/Item/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new ItemViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductId = data.ProductId,
                    BrandId = data.BrandId,
                    ItemCode = data.ItemCode,
                    Description = data.Description,
                    ProductCode = data.Product != null ? data.Product.ProductCode : "",
                    ProductDesc = data.Product != null ? data.Product.Description : "",
                    BrandCode = data.Brand != null ? data.Brand.Code : "",
                    BrandName = data.Brand != null ? data.Brand.Name : "",
                    CreatedBy = data.CreatedBy ?? "",
                    DateCreated = data.DateCreated ?? null,
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CustomerId, ItemCode, Description, ProductCode, BrandId," +
            "IsActive, CreatedBy, DateCreated")]ItemViewModel obj)
        {
            obj.UpdatedBy = CookieHelper.EmailAddress;

            var url = "api/Item/Update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, obj);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Message"] = obj.ItemCode + " is already exist! Please check and try again.";
                TempData["MessageAlert"] = "warning";
                return RedirectToAction("Index");
            }

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
                Log.Error(string.Format(Type.GetType(typeof(ItemController).Name) + "||Edit||Item ID::{0}||API Response::{1}", obj.Id, response));
                //return RedirectToAction("Index", "Item");
            }
            //return PartialView(item);
            return RedirectToAction("Index", "Item");
        }

        public async Task<ActionResult> Delete(int? id)
        {

            var obj = new ItemViewModel();
            var url = "api/Item/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new ItemViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductId = data.ProductId,
                    BrandId = data.BrandId,
                    ItemCode = data.ItemCode,
                    Description = data.Description,
                    ProductCode = data.Product != null ? data.Product.ProductCode : "",
                    ProductDesc = data.Product != null ? data.Product.Description : "",
                    BrandCode = data.Brand != null ? data.Brand.Code : "",
                    BrandName = data.Brand != null ? data.Brand.Name : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task <ActionResult> Delete(int id)
        {
            var url = $"api/Item/Delete/{id}/{_updatedBy}/";
            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Item successfully deleted!";
                TempData["MessageAlert"] = "success";
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ItemController).Name) + "||Delete||Item ID::{0}||API Response::{1}", id, response));

            }

            return RedirectToAction("Index", "Item");
        }

        public async Task<ActionResult> Enable(int? id)
        {

            var obj = new ItemViewModel();
            var url = "api/Item/GetById/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj = new ItemViewModel
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ProductId = data.ProductId,
                    BrandId = data.BrandId,
                    ItemCode = data.ItemCode,
                    Description = data.Description,
                    ProductCode = data.Product != null ? data.Product.ProductCode : "",
                    ProductDesc = data.Product != null ? data.Product.Description : "",
                    BrandCode = data.Brand != null ? data.Brand.Code : "",
                    BrandName = data.Brand != null ? data.Brand.Name : "",
                    IsActive = data.IsActive
                };
            }

            return PartialView(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Enable(int id)
        {
            var url = $"api/Item/Enable/{id}/{_updatedBy}/";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Item successfully enabled!";
                TempData["MessageAlert"] = "success";
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(ItemController).Name) + "||Enable||Item ID::{0}||API Response::{1}", id, response));

            }

            return RedirectToAction("Index", "Item");
        }
    }
}