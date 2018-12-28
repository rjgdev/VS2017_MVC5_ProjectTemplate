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
    public class DeliveryRequestLineDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ListFilterLineViewModels request)
        {
            var list = new List<DeliveryRequestLineViewModel>().AsEnumerable();
            var filteredList = new List<DeliveryRequestLineViewModel>().AsEnumerable();
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

            var url = $"api/GoodsOut/DeliveryRequest/Line/GetList/{request.IsActive}/{_customerId}/{request.HeaderId}";
            var response = await HttpClientHelper.ApiCall(url, Method.GET);
            if (response.IsSuccessful)
            {
                var result = response.Content;

                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new DeliveryRequestLineViewModel
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    IsActive = x.IsActive,
                    //LineNumber = x.LineNumber ?? 0,
                    PickType = x.PickType != null  ? x.PickType : "",
                    ProductDescription = x.Product != null ? x.Product : "",
                    Brand = x.Brand != null ? x.Brand : "",
                    ItemDescription = x.Item != null ? x.Item : "",
                    UomDescription = x.Uom != null ? x.Uom : "",
                    Quantity = x.Quantity != null ? x.Quantity : 0
                }).ToList();

                filteredList = list.Where(x => x.PickType.ToLower().Contains(searchBy) ||
                                      x.ProductDescription.ToLower().Contains(searchBy) ||
                                      x.Brand.ToLower().Contains(searchBy) ||
                                      x.ItemDescription.ToLower().Contains(searchBy) ||
                                      x.UomDescription.ToLower().Contains(searchBy) ||
                                      x.Quantity.ToString().ToLower().Contains(searchBy)).OrderBy(orderBy, StringComparison.OrdinalIgnoreCase);
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}