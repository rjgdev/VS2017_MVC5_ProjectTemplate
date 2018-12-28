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
    public class TransactionStatusDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ListFilterViewModels request)
        {
            var list = new List<TransactionStatusViewModel>().AsEnumerable();
            var filteredList = new List<TransactionStatusViewModel>().AsEnumerable();
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

            var url = $"api/Status/GetList/{request.IsActive}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new TransactionStatusViewModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Code = x.Code != null ? x.Code : "",
                    Name = x.Name != null ? x.Name : "",
                    TransactionTypeId = x.TransactionTypeId,
                    //TransTypeCode = x.TransactionType != null ? x.TransactionType.Code : "",
                    TransTypeDesc = x.TransactionType != null ? x.TransactionType.TransType : "",
                });

                filteredList = list.Where(x => x.Code.ToLower().Contains(searchBy) ||
                                      x.TransTypeDesc.ToLower().Contains(searchBy) ||
                                      x.Name.ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}