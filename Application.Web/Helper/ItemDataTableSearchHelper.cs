using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Application.Web.Models;

namespace Application.Web.Helper
{
    public class ItemDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ListFilterViewModels request)
        {
            var list = new List<ItemViewModel>().AsEnumerable();
            var filteredList = new List<ItemViewModel>().AsEnumerable();
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

            var url = $"api/Item/GetList/{request.IsActive}/{_customerId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new ItemViewModel
                {
                    Id = x.Id,
                    //CustomerId = x.CustomerId,
                    CustomerId = x.CustomerId,
                    ProductId = x.ProductId,
                    BrandId = x.BrandId,
                    ItemCode = x.ItemCode,
                    Description = x.Description,
                    ProductCode = x.Product != null ? x.Product.ProductCode : "",
                    ProductDesc = x.Product != null ? x.Product.Description : "",
                    BrandCode = x.Brand != null ? x.Brand.Code : "",
                    BrandName = x.Brand != null ? x.Brand.Name : "",
                    IsActive = x.IsActive
                });

                filteredList = list.Where(x => x.ProductDesc.ToLower().Contains(searchBy) ||
                                      x.BrandName.ToLower().Contains(searchBy) ||
                                      x.ItemCode.ToLower().Contains(searchBy) ||
                                      x.Description.ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}