using Application.Web.Helper;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;


namespace Application.Web.Controllers
{
    public class SelectListController : BaseController
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        //[HttpPost]
        //public async Task<JsonResult> Role(string term)
        //{
        //    var listOfRoles = new List<dynamic>{
        //        new { id = "Administrator", text = "Administrator" },
        //        new { id = "SuperAdmin", text = "SuperAdmin" },
        //        new { id = "WarehouseManager", text = "Warehouse Manager" },
        //        new { id = "WarehouseOperatives", text = "Warehouse Operatives" },
        //        new { id = "WarehouseSupervisor", text = "Warehouse Supervisor" },
        //        new { id = "WarehouseAdministrator", text = "Warehouse Administrator" },
        //        new { id = "Warehouse Staff", text = "Warehouse Staff" },

        //    }.AsEnumerable();

        //    if (!string.IsNullOrEmpty(term))
        //    {
        //        listOfRoles = listOfRoles.Where(x => x.text.ToLower().Contains(term.ToLower()));
        //    }

        //    return Json(listOfRoles);
        //}

        [HttpPost]
        public async Task<JsonResult> IsActiveSelectList(string term)
        {
            var list = new List<dynamic>{
                new { id = "true", text = "Yes" },
                new { id = "false", text = "No" },
            }.AsEnumerable();

            if (!string.IsNullOrEmpty(term))
            {
                list = list.Where(x => x.text.ToLower().Contains(term.ToLower()));
            }

            return Json(list);            
        }

        [HttpPost]
        public async Task<JsonResult> StatusSelectList(string term)
        {

            var list = new List<dynamic>{
                new { id = "ALL", text = "ALL" },
                new { id = "For Receiving", text = "For Receiving" },
                new { id = "Received", text = "Received" },
                new { id = "Completed", text = "Completed" }

            }.AsEnumerable();

            if (!string.IsNullOrEmpty(term))
            {
                list = list.Where(x => x.text.ToLower().Contains(term.ToLower()));
            }
            return Json(list);
        }

        [HttpPost]
        public async Task<JsonResult> StatusSelectListForReceipt(string term)
        {

            var list = new List<dynamic>{
                new { id = "ALL", text = "ALL" },
                new { id = "Received", text = "Received" },
                new { id = "Completed", text = "Completed" }

            }.AsEnumerable();

            if (!string.IsNullOrEmpty(term))
            {
                list = list.Where(x => x.text.ToLower().Contains(term.ToLower()));
            }
            return Json(list);
        }

        [HttpPost]
        public async Task<JsonResult> StatusSelectListForDeliveryRequest(string term)
        {
            var list = new List<StatusViewModel>();

            var url = $"api/Status/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<StatusViewModel>>(result);
                list = obj;

                var status = new List<dynamic> { new { id = 0, text = "ALL"} };
                    
                status.AddRange(list.Where(x => (x.Name?.ToLower().Contains(term.ToLower()) ?? false)
                                                && (x.Id == 13 || x.Id == 14 || x.Id == 15))
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Name

                    }));


                return Json(status);
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> CustomerClientList(string term)
        {
            var list = new List<CustomerClientViewModel>();
            var url = $"api/customerclient/getlist/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<CustomerClientViewModel>>(result);

                return Json(list.Where(x => x.Name?.ToLower().Contains(term.ToLower()) ?? false)
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Name

                    }).ToList());
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> VendorList(string term)
        {
            var list = new List<VendorViewModel>();
            var url = $"api/Vendor/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            //term = "";

            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<VendorViewModel>>(result);

                var warehouse = list.Where(x => (x.VendorName?.ToLower().Contains(term.ToLower()) ?? false))
                    .Select(x => new
                    {
                        id = x.VendorCode,
                        text = x.VendorName

                    }).ToList();

                return Json(warehouse);
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> VendorIdList(string term)
        {
            var list = new List<VendorViewModel>();
            var url = $"api/Vendor/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<VendorViewModel>>(result);
                list = obj;

                var warehouse = list.Where(x => (x.VendorName?.ToLower().Contains(term.ToLower()) ?? false))
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.VendorName

                    }).ToList();

                return Json(warehouse);
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> WarehouseList(string term)
        {
            var list = new List<WarehouseViewModel>();
            var url = $"api/Warehouse/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            //term = "";

            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<WarehouseViewModel>>(result);

                var warehouse = list.Where(x => (x.Description?.ToLower().Contains(term.ToLower()) ?? false))
                    .Select(x => new
                    {
                        id = x.WarehouseCode,
                        text = x.Description,
                        otherField = x.Address1 ?? "Sample City"

                    }).ToList();

                return Json(warehouse);
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> WarehouseIdList(string term)
        {
            var list = new List<WarehouseViewModel>();
            var url = $"api/Warehouse/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<WarehouseViewModel>>(result);
                list = obj;

                var warehouse = list.Where(x => (x.Description?.ToLower().Contains(term.ToLower()) ?? false))
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Description,
                        otherField = x.Address1 ?? "Sample City"

                    }).ToList();

                return Json(warehouse);
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> LocationList(string term)
        {
            var list = new List<LocationViewModel>();
            var url = $"api/Location/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<LocationViewModel>>(result);
                list = obj;
            }

            return Json(list);
        }

        [HttpPost]
        public async Task<JsonResult> LocationListByWarehouseCode(string term, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var list = new List<LocationViewModel>();
                var url = $"api/Location/GetByWarehouseCode/{code}/{true}/{_customerId}";

                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<List<dynamic>>(result);

                    list = obj.Select(x => new LocationViewModel
                    {
                        Id = x.Id,
                        LocationCode = x.Code,
                        Description = x.Description
                    }).ToList();

                    var location = list.Where(x => (x.Description?.ToLower().Contains(term.ToLower()) ?? false))
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Description

                        }).ToList();

                    return Json(location);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> ProductList(string term)
        {

            var list = new List<ProductViewModel>();
            var url = $"api/Product/GetList/{true}/{_customerId}";
            //term = "";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
                list = obj;

                var product = list.Where(x => x.Description?.ToLower().Contains(term.ToLower()) ?? false)
                    .Select(x => new
                    {
                        id = x.ProductCode,
                        text = x.Description

                    }).ToList();

                return Json(product);
            }

            return null;
        }

        [HttpPost]
        public async Task<JsonResult> ProductIdList(string term)
        {

            var list = new List<ProductViewModel>();
            var url = $"api/Product/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
                list = obj;

                var product = list.Where(x => x.Description?.ToLower().Contains(term.ToLower()) ?? false)
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Description

                    }).ToList();

                return Json(product);
            }

            return null;
        }
        
        [HttpPost]
        public async Task<JsonResult> BrandListByProductCode(string term, string code)
        {

            if (!string.IsNullOrEmpty(code))
            {
                var list = new List<BrandViewModel>();
                var url = $"api/brand/GetByProductCode/{code}/{true}/{_customerId}";
                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<List<BrandViewModel>>(result);
                    list = obj;

                    var product = list.Where(x => (x.Name?.ToLower().Contains(term.ToLower()) ?? false))
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Name

                        }).ToList();

                    return Json(product);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> BrandListByProductId(string term, int? id)
        {

            if (id != null)
            {
                var list = new List<BrandViewModel>();
                var url = $"api/brand/GetByProductId/{id}";
                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<List<BrandViewModel>>(result);
                    list = obj;

                    var product = list.Where(x => (x.Name?.ToLower().Contains(term.ToLower()) ?? false))
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Name

                        }).ToList();

                    return Json(product);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> ItemDescBrandByItemCode(int? itemId)
        {

            if (itemId != null && itemId != 0)
            {
                
                var url = $"api/Item/GetItemDescByItemCode/{itemId}";
                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<dynamic>(result);
                   
                    return Json(result);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> ItemCodeByProductCode(string term, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                //term = "";
                var list = new List<ItemViewModel>();
                var url = $"api/Item/GetSelectListByProductCode/{code}";
                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<List<dynamic>>(result);

                    list = obj.Select(x => new ItemViewModel
                    {
                        Id = x.Id,
                        ItemCode = x.Code,
                        Description = x.Description
                    }).ToList();

                    var product = list.Where(x => x.ItemCode?.ToLower().Contains(term.ToLower()) ?? false)
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.ItemCode

                        }).ToList();

                    return Json(product);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> ItemList(string term)
        {
            var list = new List<ItemViewModel>();
            var url = $"api/Item/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<ItemViewModel>>(result);
                list = obj;
            }

            return Json(list);
        }

        [HttpPost]
        public async Task<JsonResult> ItemListByBrandId(string term, int? id)
        {
            if (id != null && id != 0)
            {
                var list = new List<ItemViewModel>();
                var url = $"api/Item/GetSelectListByBrandId/{id}";
                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<List<dynamic>>(result);

                    list = obj.Select(x => new ItemViewModel
                    {
                        Id = x.Id,
                        ItemCode = x.Code,
                        Description = x.Description
                    }).ToList();

                    var item = list.Where(x => x.ItemCode?.ToLower().Contains(term.ToLower()) ?? false)
                        .Select(x =>
                       new
                       {
                           id = x.Id,
                           text = x.ItemCode,
                           otherField = x.Description
                       }).ToList();

                    return Json(item);
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<JsonResult> ItemListByProductId(string term, int? id)
        {
            if (id != null && id != 0)
            {
                var list = new List<ItemViewModel>();
                var url = $"api/Item/GetSelectListByProductId/{id}";
                var response = await HttpClientHelper.ApiCall(url, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var obj = JsonConvert.DeserializeObject<List<dynamic>>(result);

                    list = obj.Select(x => new ItemViewModel
                    {
                        Id = x.Id,
                        ItemCode = x.Code,
                        Description = x.Description
                    }).ToList();

                    var item = list.Where(x => x.ItemCode?.ToLower().Contains(term.ToLower()) ?? false)
                        .Select(x =>
                       new
                       {
                           id = x.Id,
                           text = x.ItemCode,
                           otherField = x.Description
                       }).ToList();

                    return Json(item);
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<JsonResult> UomList(string term)
        {
            var list = new List<UomViewModel>();
            var url = $"api/Uom/GetList/{true}/{_customerId}";
            //term = "";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<UomViewModel>>(result);
                list = obj;

                var uom = list.Where(x => x.Description?.ToLower().Contains(term.ToLower()) ?? false)
                   .Select(x => new
                   {
                       id = x.Code,
                       text = x.Description

                   }).ToList();

                return Json(uom);
            }

            return null;

        }


        [HttpPost]
        public async Task<JsonResult> UomIdList(string term)
        {
            var list = new List<UomViewModel>();
            var url = $"api/Uom/GetList/{true}/{_customerId}";
            //term = "";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<UomViewModel>>(result);
                list = obj;

                var uom = list.Where(x => x.Description?.ToLower().Contains(term.ToLower()) ?? false)
                   .Select(x => new
                   {
                       id = x.Id,
                       text = x.Description

                   }).ToList();

                return Json(uom);
            }

            return null;

        }


        [HttpPost]
        public async Task<JsonResult> TransactionTypesList(string term)
        {
            var list = new List<TransactionTypesViewModel>();
            var url = $"api/TransactionTypes/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<TransactionTypesViewModel>>(result);
                list = obj;

                var transTypes = list.Where(x => x.TransType?.ToLower().Contains(term.ToLower()) ?? false)
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.TransType

                    }).ToList();

                return Json(transTypes);
            }

            return null;
        }

        [HttpPost]
        public async Task<JsonResult> StatusList(string term)
        {
            var list = new List<StatusViewModel>();

            var url = $"api/Status/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<StatusViewModel>>(result);
                list = obj;

                var status = list.Where(x => x.Name?.ToLower().Contains(term.ToLower()) ?? false)
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Name

                    }).ToList();

                return Json(status);
            }
            return null;
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> HaulierList(string term)
        {
            var list = new List<HaulierViewModel>();
            var url = $"api/Haulier/GetList/{true}/{_customerId}";
            //term = "";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<HaulierViewModel>>(result);

                return Json(list.Where(x => x.Name?.ToLower().Contains(term.ToLower()) ?? false)
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Name

                    }).ToList());
            }
            return null;
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> PickTypeList(string term)
        {
            var list = new List<PickTypeViewModels>();
            var url = $"api/PickType/GetList/{true}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var customerId = int.Parse(CookieHelper.CustomerId);
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<PickTypeViewModels>>(result);

                return Json(list.Where(x => (x.Description?.ToLower().Contains(term.ToLower()) ?? false)
                                          && x.CustomerId == customerId
                    )
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Description

                    }).ToList());
            }
            return Json(null);
        }
    }
}