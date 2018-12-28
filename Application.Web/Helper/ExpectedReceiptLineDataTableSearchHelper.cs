using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Application.Web.Helper
{
    public class ExpectedReceiptLineDataTableSearchHelper
    {
         private int _customerId = int.Parse(CookieHelper.CustomerId);

        public async Task<dynamic> SearchFunction(ListFilterLineViewModels request)
        {
            var list = new List<ExpectedReceiptViewModel.ExpectedReceiptLine>().AsEnumerable();
            var filteredList = new List<ExpectedReceiptViewModel.ExpectedReceiptLine>().AsEnumerable();
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

            var url = $"api/GoodsIn/ExpectedReceipt/GetByExpectedReceipt/{request.HeaderId}/{request.IsActive}/{_customerId}/";

            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;

                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new ExpectedReceiptViewModel.ExpectedReceiptLine
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    ExpectedReceiptId = x.ExpectedReceiptId,
                    Quantity = x.Quantity,
                    BrandId = x.BrandId,
                    ProductId = x.ProductId,
                    BrandName = x.Brand != null ? x.Brand.Name : "",
                    Batch = x.Batch,
                    ItemCode = x.ItemCode,
                    ItemDescription = x.ItemDescription != null ? x.ItemDescription : "",
                    ExpiryDate = x.ExpiryDate,
                    UomDescription = x.Uom != null ? x.Uom.Description : "",
                    ProductCode = x.Product != null ? x.Product.Description : "",
                    StatusCode = x.ExpectedReceipt != null ? x.ExpectedReceipt.Status.Name : "",
                    GoodsReceivedNumber = x.ExpectedReceipt != null ? x.ExpectedReceipt.GoodsReceivedNumber : ""

                }).ToList();

                filteredList = list.Where(x => (x.ExpiryDate?.ToString("MMM. dd, yyyy").ToLower().Contains(searchBy) ?? false) ||
                                     x.ItemCode.ToLower().Contains(searchBy) ||
                                     x.BrandName.ToLower().Contains(searchBy) ||
                                     x.ItemDescription.ToLower().Contains(searchBy) ||
                                     x.Quantity.ToString().ToLower().Contains(searchBy) ||
                                     x.UomDescription.ToLower().Contains(searchBy));
            }

            totalResultCount = list.Count();
            filteredResultCount = filteredList.Count();
            filteredList = filteredList.Skip(skip).Take(take);

            return new { filteredList, filteredResultCount, totalResultCount };
        }
    }
}