using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace Application.Web.Helper
{
    public class DeliveryRequestDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(DeliveryRequestFilterViewModels request)
        {
            var list = new List<DeliveryRequestViewModel>().AsEnumerable();
            var filteredList = new List<DeliveryRequestViewModel>().AsEnumerable();
            int filteredResultCount = 0;
            int totalResultCount = 0;

            var searchBy = request.search?.value ?? "";
            searchBy = searchBy?.ToLower() ?? "";
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

            var url = $"api/GoodsOut/DeliveryRequest/GetList/{request.IsActive}/{_customerId}/{request.StatusId}";

            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.IsSuccessful)
            {
                var result = response.Content;

                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new DeliveryRequestViewModel
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    IsActive = x.IsActive,
                    CustomerClientName = x.CustomerClient != null ? x.CustomerClient : "",
                    DeliveryRequestCode = x.DeliveryRequestCode != null ? x.DeliveryRequestCode : "",
                    HaulierName = x.Haulier != null ? x.Haulier : "",
                    RequestedDate = x.RequestedDate != null ? x.RequestedDate : DateTime.MinValue,
                    RequestType = x.RequestType != null ? x.RequestType : "",
                    SalesOrderRef = x.SalesOrderRef != null ? x.SalesOrderRef : "",
                    WarehouseDescription = x.Warehouse != null ? x.Warehouse : "",
                    StatusId = x.StatusId,
                    StatusName = x.Status != null ? x.Status : ""
                }).ToList();

                filteredList = list.Where(x => x.CustomerClientName.ToLower().Contains(searchBy) ||
                                      x.DeliveryRequestCode.ToLower().Contains(searchBy) ||
                                      x.HaulierName.ToLower().Contains(searchBy) ||
                                      x.RequestedDate.ToString("MMM. dd, yyyy").ToLower().Contains(searchBy) ||
                                      x.RequestType.ToLower().Contains(searchBy) ||
                                      x.SalesOrderRef.ToLower().Contains(searchBy) ||
                                      x.WarehouseDescription.ToLower().Contains(searchBy) ||
                                      x.StatusName.ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}