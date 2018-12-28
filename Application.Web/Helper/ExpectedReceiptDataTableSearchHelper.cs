using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using static Application.Web.Models.ViewModels.ExpectedReceiptViewModel;

namespace Application.Web.Helper
{
    public class ExpectedReceiptDataTableSearchHelper
    {
       
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ExpectedReceiptFilterViewModels request)
        {
            var list = new List<ExpectedReceiptViewModel.ExpectedReceipt>().AsEnumerable();
            var filteredList = new List<ExpectedReceiptViewModel.ExpectedReceipt>().AsEnumerable();
            int filteredResultCount = 0;
            int totalResultCount = 0;

            var searchBy = request.search.value?.ToLower() ?? "" ?? "";

            var take = request.length;
            var skip = request.start;

            var orderBy = "Id";
            var orderDir = true;

            if (request.order != null)
            {
                orderDir = request.order[0].dir.ToLower().Equals("asc");
                orderBy = request.columns[request.order[0].column].data;
                orderBy = (orderBy.Equals("actions", StringComparison.OrdinalIgnoreCase) ? "Id" : orderBy) + (orderDir ? "" : " desc");
            }

            var url = "";

            if (request.StatusCode.Equals("Received"))
            {
                url = $"api/GoodsIn/ExpectedReceipt/GetReceived/{request.IsActive}/{_customerId}/"; 
            }
            else if (request.StatusCode.Equals("Completed"))
            {
                url = $"api/GoodsIn/ExpectedReceipt/GetCompleted/{request.IsActive}/{_customerId}/";
            }
            else if (request.StatusCode.Equals("For Receiving"))
            {
                url = $"api/GoodsIn/ExpectedReceipt/GetForReceiving/{request.IsActive}/{_customerId}/";
            }
            else if (request.StatusCode.Equals("ALL"))
            {
                url = $"api/GoodsIn/ExpectedReceipt/GetList/{request.IsActive}/{_customerId}/";
            }



            var response = await HttpClientHelper.ApiCall(url, Method.GET);


            if (response.IsSuccessful)
            {
                var result = response.Content;

                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new ExpectedReceipt
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    WarehouseDescription = x.Warehouse != null ? x.Warehouse.Description : "",
                    ExpectedReceiptDate = x.ExpectedReceiptDate,
                    GoodsReceivedNumber = x.GoodsReceivedNumber ?? "",
                    ReferenceNumber = x.ReferenceNumber ?? "",
                    //Received = x.Received,
                    Comments = x.Comments ?? "",
                    Address = x.Warehouse != null ? x.Warehouse.Address1 : "",
                    WarehouseCode = x.Warehouse != null ? x.Warehouse.Description : "",
                    ReceivedBy = x.ReceivedBy != null ? x.ReceivedBy : "",
                    ReceivedDate = x.ReceivedDate,
                    Supplier = x.Supplier ?? "",
                    HaulierName = x.Haulier != null ? x.Haulier.Name : "",
                    HaulierCode = x.Haulier != null ? x.Haulier.HaulierCode : "",
                    StatusCode = x.Status != null ? x.Status.Name : "",
                    Planned = x.Planned


                }).Where(x => x.Planned == true).ToList();

                filteredList = list.Where(x => 
                                     x.ExpectedReceiptDate.ToString("MMM. dd, yyyy").ToLower().Contains(searchBy) ||
                                     x.GoodsReceivedNumber.ToLower().Contains(searchBy) || 
                                     x.ReferenceNumber.ToLower().Contains(searchBy) ||
                                     (x.ReceivedDate?.ToString("MMM. dd, yyyy").ToLower().Contains(searchBy) ?? false) ||
                                     x.ReceivedBy.ToLower().Contains(searchBy) ||
                                     x.StatusCode.ToLower().Contains(searchBy) ||
                                     //x.WarehouseDescription.ToLower().Contains(searchBy) ||
                                     x.Address.ToLower().Contains(searchBy) ||
                                     //x.Supplier.ToLower().Contains(searchBy) 
                                     x.HaulierName.ToLower().Contains(searchBy)
                                     ).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);


            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}