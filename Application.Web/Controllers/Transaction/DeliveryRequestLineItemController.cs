using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.Web.Models;
using Application.Web.Models.ViewModels;
using Application.Web.Helper;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Web.Controllers.Transaction
{
    public class DeliveryRequestLineItemController : BaseController
    {


        // GET: DeliveryRequestLineItem
        [Authorize]
        public async Task<ActionResult> Index(long? id,long? deliveryRequestId, long? productId)
        {

            var list = new List<DeliveryRequestLineItemViewModel>();
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/GetListByLineId/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<DeliveryRequestLineItemViewModel>>(result);
                list = obj;
            }

            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<dynamic>>(result).Select(x => new DeliveryRequestLineItemViewModel
                {
                    Id = x.Id,
                    DeliveryRequestLineId = x.DeliveryRequestLineId,
                    ItemId = x.ItemId,
                    ItemDescription = x.Item.Description,

                }).ToList();
            }
            ViewBag.ProductId = productId;
            ViewBag.DeliveryRequestLineId = id;
            ViewBag.DeliveryRequestId = deliveryRequestId;
            return View(list);
        }

        // GET: DeliveryRequestLineItem/Details/5
        [Authorize]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineItemViewModel();
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.DeliveryRequestLineId = data.DeliveryRequestLineId;
                obj.ItemId = data.ItemId;
                obj.ItemDescription = data.Item.Description;

            }

            return PartialView(obj);
        }

        // GET: DeliveryRequestLineItem/Create
        [Authorize]
        public ActionResult Create(long? id,long? deliveryRequestId, long? productId)
        {
            var obj = new DeliveryRequestLineItemViewModel();
            obj.DeliveryRequestLineId = id;
            obj.ProductId = productId;
            obj.DeliveryRequestId = deliveryRequestId;
            return PartialView(obj);
        }

        // POST: DeliveryRequestLineItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DeliveryRequestLineId,ItemId,ProductId,DeliveryRequestId")] DeliveryRequestLineItemViewModel deliveryRequestLineItemViewModel)
        {
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/Add";

            var response = await HttpClientHelper.ApiCall(url, Method.POST, deliveryRequestLineItemViewModel);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Created";
                return RedirectToAction("Index", new { id = deliveryRequestLineItemViewModel.DeliveryRequestLineId, deliveryRequestLineItemViewModel.DeliveryRequestId, deliveryRequestLineItemViewModel.ProductId });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineItemController).Name) + "||Create||DeliveryRequestLine ID::{0}||API Response::{1}", deliveryRequestLineItemViewModel.Id, response));
                return RedirectToAction("Index", new { id = deliveryRequestLineItemViewModel.DeliveryRequestLineId, deliveryRequestLineItemViewModel.DeliveryRequestId, deliveryRequestLineItemViewModel.ProductId });
            }
        }

        // GET: DeliveryRequestLineItem/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(long? id,long? deliveryRequestId, long? productId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineItemViewModel();
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.DeliveryRequestLineId = data.DeliveryRequestLineId;
                obj.ItemId = data.ItemId;
                obj.ItemDescription = data.Item.Description;
                obj.ProductId = productId;
                obj.DeliveryRequestId = deliveryRequestId;
            }

            return PartialView(obj);
        }

        // POST: DeliveryRequestLineItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DeliveryRequestLineId,ItemId,ProductId,DeliveryRequestId")] DeliveryRequestLineItemViewModel deliveryRequestLineItemViewModel)
        {
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, deliveryRequestLineItemViewModel);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Updated";
                return RedirectToAction("Index", new { id = deliveryRequestLineItemViewModel.DeliveryRequestLineId, deliveryRequestLineItemViewModel.DeliveryRequestId, deliveryRequestLineItemViewModel.ProductId });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineItemController).Name) + "||Update||DeliveryRequestLineItem ID::{0}||API Response::{1}", deliveryRequestLineItemViewModel.Id, response));
                return RedirectToAction("Index", new { id = deliveryRequestLineItemViewModel.DeliveryRequestLineId, deliveryRequestLineItemViewModel.DeliveryRequestId, deliveryRequestLineItemViewModel.ProductId });
            }
        }

        // GET: DeliveryRequestLineItem/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(long? id,long? deliveryRequestId, long? productId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DeliveryRequestLineItemViewModel();
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var data = JsonConvert.DeserializeObject<dynamic>(result);

                obj.Id = data.Id;
                obj.DeliveryRequestLineId = data.DeliveryRequestLineId;
                obj.ItemId = data.ItemId;
                obj.ItemDescription = data.Item.Description;
                obj.ProductId = productId;
                obj.DeliveryRequestId = deliveryRequestId;
            }

            return PartialView(obj);
        }

        // POST: DeliveryRequestLineItem/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id, long? DeliveryRequestLineId, long? DeliveryRequestId,long? ProductId)
        {
            var url = "api/GoodsOut/CustomerDespatch/Line/Item/delete/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Deleted";
                return RedirectToAction("Index",new { id = DeliveryRequestLineId, deliveryRequestId = DeliveryRequestId, productId = ProductId });
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestLineItemController).Name) + "||Delete||DeliveryRequestLineItem ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index", new { id = DeliveryRequestLineId, deliveryRequestId = DeliveryRequestId, productId = ProductId });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> ItemList(long productId ,string term)
        {
            var list = new List<ItemViewModel>();
            var url = "api/item/getselectlist/" + productId;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                list = JsonConvert.DeserializeObject<List<ItemViewModel>>(result);

                return Json(list.Where(x => x.Description?.ToLower().StartsWith(term ?? "", StringComparison.OrdinalIgnoreCase) ?? false)
                    .Select(x => new
                    {
                        id = x.Id,
                        text = x.Description

                    }).ToList());
            }
            return null;
        }

    }
}
