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
using RestSharp;
using Application.Web.Helper;
using Newtonsoft.Json;

namespace Application.Web.Controllers.Transaction
{
    public class StockAssignController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockAssign
        public async Task<ActionResult> Index()
        {
            var list = new List<StockAssignViewModel>();
            var url = "api/stockasign/GetList/" + 100;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<StockAssignViewModel>>(result);
                list = obj;
            }

            return View(list);
        }

        // GET: StockAssign/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new StockAssignViewModel();
            var url = "api/stockasign/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<StockAssignViewModel>(result);
            }

            return PartialView(obj);
        }

        // GET: StockAssign/Create
        public ActionResult Create()
        {
            //ViewBag.ItemId = new SelectList(db.ItemViewModels, "Id", "Description");
            //ViewBag.LocationId = new SelectList(db.LocationViewModels, "Id", "Description");
            //ViewBag.ProductId = new SelectList(db.ProductViewModels, "Id", "Description");
            //ViewBag.WarehouseId = new SelectList(db.WarehouseViewModels, "Id", "WarehouseCode");
            return PartialView();

        }

        // POST: StockAssign/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductId,ItemId,WarehouseId,LocationId,TotalQtyStock")] StockAssignViewModel stockAssignViewModel)
        {
            var url = "api/stockasign/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, stockAssignViewModel);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Created";
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DeliveryRequestViewModel).Name) + "||Create||StockAssign ID::{0}||API Response::{1}", stockAssignViewModel.Id, response));
                return RedirectToAction("Index");
            }
            //ViewBag.ItemId = new SelectList(db.ItemViewModels, "Id", "Description", stockAssignViewModel.ItemId);
            //ViewBag.LocationId = new SelectList(db.LocationViewModels, "Id", "Description", stockAssignViewModel.LocationId);
            //ViewBag.ProductId = new SelectList(db.ProductViewModels, "Id", "Description", stockAssignViewModel.ProductId);
            //ViewBag.WarehouseId = new SelectList(db.WarehouseViewModels, "Id", "WarehouseCode", stockAssignViewModel.WarehouseId);
        }

        // GET: StockAssign/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new StockAssignViewModel();
            var url = "api/stockasign/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<StockAssignViewModel>(result);
            }

            return PartialView(obj);
        }

        // POST: StockAssign/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductId,ItemId,WarehouseId,LocationId,TotalQtyStock")] StockAssignViewModel stockAssignViewModel)
        {
            var url = "api/stockassign/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, stockAssignViewModel);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Updated";
                return RedirectToAction("Index", "DeliveryRequestLines");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(StockAssignViewModel).Name) + "||Update||StockAssign ID::{0}||API Response::{1}", stockAssignViewModel.Id, response));
                return PartialView(stockAssignViewModel);
            }

            //ViewBag.ItemId = new SelectList(db.ItemViewModels, "Id", "Description", stockAssignViewModel.ItemId);
            //ViewBag.LocationId = new SelectList(db.LocationViewModels, "Id", "Description", stockAssignViewModel.LocationId);
            //ViewBag.ProductId = new SelectList(db.ProductViewModels, "Id", "Description", stockAssignViewModel.ProductId);
            //ViewBag.WarehouseId = new SelectList(db.WarehouseViewModels, "Id", "WarehouseCode", stockAssignViewModel.WarehouseId);
            
        }

        // GET: StockAssign/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new StockAssignViewModel();
            var url = "api/stockasign/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<StockAssignViewModel>(result);
            }

            return PartialView(obj);
        }

        // POST: StockAssign/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var url = "api/stockassign/delete/" + id;

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Deleted";
                return RedirectToAction("Index", "StockAssign");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(StockAssignViewModel).Name) + "||Delete||StockAssign ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index");
            }
        }
      
    }
}
