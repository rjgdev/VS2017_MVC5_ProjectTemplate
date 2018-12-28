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
using RestSharp;
using Newtonsoft.Json;

namespace Application.Web.Controllers.Transaction
{
    public class DispatchController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string _updatedBy = CookieHelper.EmailAddress;

        // GET: Dispatch
        public async Task<ActionResult> Index()
        {
            var list = new List<DispatchViewModel>();
            var url = "api/dispatch/GetList/" + 100;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var obj = JsonConvert.DeserializeObject<List<DispatchViewModel>>(result);
                list = obj;
            }

            return View(list);
        }

        // GET: Dispatch/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DispatchViewModel();
            var url = "api/dispatch/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<DispatchViewModel>(result);
            }

            return PartialView(obj);
        }

        // GET: Dispatch/Create
        public ActionResult Create()
        {
            //ViewBag.DeliveryRequestId = new SelectList(db.DeliveryRequestViewModels, "Id", "Requestor");
            //ViewBag.ItemId = new SelectList(db.ItemViewModels, "Id", "Description");
            //ViewBag.ProductId = new SelectList(db.ProductViewModels, "Id", "Description");
            //ViewBag.WarehouseId = new SelectList(db.WarehouseViewModels, "Id", "WarehouseCode");

            return PartialView();
        }

        // POST: Dispatch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ItemId,ProductId,WarehouseId,DeliveryRequestId")] DispatchViewModel dispatchViewModel)
        {
            var url = "api/dispatch/add";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, dispatchViewModel);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Created";
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DispatchViewModel).Name) + "||Create||Dispatch ID::{0}||API Response::{1}", dispatchViewModel.Id, response));
                return RedirectToAction("Index");
            }

            //ViewBag.DeliveryRequestId = new SelectList(db.DeliveryRequestViewModels, "Id", "Requestor", dispatchViewModel.DeliveryRequestId);
            //ViewBag.ItemId = new SelectList(db.ItemViewModels, "Id", "Description", dispatchViewModel.ItemId);
            //ViewBag.ProductId = new SelectList(db.ProductViewModels, "Id", "Description", dispatchViewModel.ProductId);
            //ViewBag.WarehouseId = new SelectList(db.WarehouseViewModels, "Id", "WarehouseCode", dispatchViewModel.WarehouseId);
            return View(dispatchViewModel);
        }

        // GET: Dispatch/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DispatchViewModel();
            var url = "api/dispatch/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<DispatchViewModel>(result);
            }

            return PartialView(obj);
        }

        // POST: Dispatch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ItemId,ProductId,WarehouseId,DeliveryRequestId")] DispatchViewModel dispatchViewModel)
        {
            var url = "api/dispatch/update";
            var response = await HttpClientHelper.ApiCall(url, Method.PUT, dispatchViewModel);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Updated";
                return RedirectToAction("Index");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DispatchViewModel).Name) + "||Edit||Dispatch ID::{0}||API Response::{1}", dispatchViewModel.Id, response));
                return RedirectToAction("Index");
            }

            //ViewBag.DeliveryRequestId = new SelectList(db.DeliveryRequestViewModels, "Id", "Requestor", dispatchViewModel.DeliveryRequestId);
            //ViewBag.ItemId = new SelectList(db.ItemViewModels, "Id", "Description", dispatchViewModel.ItemId);
            //ViewBag.ProductId = new SelectList(db.ProductViewModels, "Id", "Description", dispatchViewModel.ProductId);
            //ViewBag.WarehouseId = new SelectList(db.WarehouseViewModels, "Id", "WarehouseCode", dispatchViewModel.WarehouseId);0-
        }

        // GET: Dispatch/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = new DispatchViewModel();
            var url = "api/dispatch/getbyid/" + id;
            var response = await HttpClientHelper.ApiCall(url, Method.GET);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                obj = JsonConvert.DeserializeObject<DispatchViewModel>(result);
            }

            return PartialView(obj);
        }

        // POST: Dispatch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var url = $"api/Dispatch/delete/{id}/{_updatedBy}/";

            var response = await HttpClientHelper.ApiCall(url, Method.DELETE);

            if (response.IsSuccessful)
            {
                TempData["Message"] = "Successfully Deleted";
                return RedirectToAction("Index", "Dispatch");
            }
            else
            {
                Error("An error has occurred");
                Log.Error(string.Format(Type.GetType(typeof(DispatchViewModel).Name) + "||Delete||Dispatch ID::{0}||API Response::{1}", id, response));
                return RedirectToAction("Index");
            }
        }
        
    }
}
