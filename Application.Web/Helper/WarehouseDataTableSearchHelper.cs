using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;

namespace Application.Web.Helper
{
    public class WarehouseDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ListFilterViewModels request)
        {
            var list = new List<WarehouseViewModel>().AsEnumerable();
            var filteredList = new List<WarehouseViewModel>().AsEnumerable();
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

            var url = $"api/warehouse/GetList/{request.IsActive}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new WarehouseViewModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    CustomerId = x.CustomerId,
                    WarehouseCode = x.WarehouseCode != null ? x.WarehouseCode : "",
                    Description = x.Description != null ? x.Description : "",
                    Address1 = x.Address1 != null ? x.Address1 : "",
                    Address2 = x.Address2 != null ? x.Address2 : "",
                });

                filteredList = list.Where(x => x.WarehouseCode.ToLower().Contains(searchBy) ||
                                               x.Address1.ToLower().Contains(searchBy) ||
                                               x.Address2.ToLower().Contains(searchBy) ||
                                      x.Description.ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}