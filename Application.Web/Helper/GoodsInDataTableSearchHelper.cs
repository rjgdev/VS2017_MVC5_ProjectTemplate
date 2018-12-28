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
    public class GoodsInDataTableSearchHelper
    {

         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ExpectedReceiptFilterViewModels request, bool IsPlanned)
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

            DateTime dt;
            DateTime? sDateFrom = null;
            if (DateTime.TryParse(request.from, out dt))
            {
                sDateFrom = Convert.ToDateTime(request.from);
            }
            DateTime? sDateTo = null;
            if (DateTime.TryParse(request.to, out dt))
            {
                sDateTo = Convert.ToDateTime(request.to).AddDays(1);
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
            else if (request.StatusCode.Equals("For Receiving") && IsPlanned )
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
                    ReferenceNumber = x.ReferenceNumber != null ? x.ReferenceNumber : "",
                    AutoReferenceNumber = x.AutoReferenceNumber != null ? x.AutoReferenceNumber : "",
                    //Received = x.Received,
                    Comments = x.Comments ?? "",
                    Address = x.Warehouse != null ? x.Warehouse.Address1 : "",
                    WarehouseCode = x.Warehouse != null ? x.Warehouse.Description : "",
                    ReceivedBy = x.ReceivedBy != null ? x.ReceivedBy : "",
                    ReceivedDate = x.ReceivedDate,
                    Supplier = x.Supplier != null ? x.Supplier : "",
                    HaulierName = x.Haulier != null ? x.Haulier.Name : "",
                    HaulierCode = x.Haulier != null ? x.Haulier.HaulierCode : "",
                    StatusCode = x.Status != null ? x.Status.Name : "",
                    Planned = x.Planned


                }).Where(x => x.Planned == IsPlanned).ToList();

                filteredList = list.Where(x =>
                                     x.ExpectedReceiptDate.ToString("MMM. dd, yyyy").ToLower().Contains(searchBy) ||
                                     x.AutoReferenceNumber.ToLower().Contains(searchBy) ||
                                     x.GoodsReceivedNumber.ToLower().Contains(searchBy) ||
                                     x.ReferenceNumber.ToLower().Contains(searchBy) ||
                                     (x.ReceivedDate?.ToString("MMM. dd, yyyy").ToLower().Contains(searchBy) ?? false) ||
                                     x.ReceivedBy.ToLower().Contains(searchBy) ||
                                     x.StatusCode.ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);


            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}