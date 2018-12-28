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
    public class CustomerDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ListFilterViewModels request)
        {
            var list = new List<CustomerViewModel>().AsEnumerable();
            var filteredList = new List<CustomerViewModel>().AsEnumerable();
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

            var url = $"api/Customer/GetList/{request.IsActive}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new CustomerViewModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Domain = x.Domain ?? "",
                    CompanyName = x.CompanyName ?? "",
                    EmailAddress = x.EmailAddress ?? "",
                    FirstName = x.FirstName ?? "",
                    MiddleName = x.MiddleName != null ? x.MiddleName : "",
                    LastName = x.LastName ?? "",
                    ContactNo = x.ContactNo != null ? x.ContactNo : ""
                });

                filteredList = list.Where(x => x.Domain.ToLower().Contains(searchBy) ||
                                               x.CompanyName.ToLower().Contains(searchBy) ||
                                               x.EmailAddress.ToLower().Contains(searchBy) ||
                                               x.ContactNo.ToLower().Contains(searchBy) ||
                                               x.FirstName.ToLower().Contains(searchBy) ||
                                               x.MiddleName.ToLower().Contains(searchBy) ||
                                               x.LastName.ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
                                               //(x.FirstName + (!string.IsNullOrEmpty(x.MiddleName) ? " " + x.MiddleName[0] + ". " : " ") + x.LastName)
                                               //.Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}