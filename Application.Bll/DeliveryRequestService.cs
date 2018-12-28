using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Data.Repository;
using Application.Model;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System;
using System.Linq;
using Newtonsoft.Json;
using Application.Bll.Models;

namespace Application.Bll
{
    public class DeliveryRequestService : IDeliveryRequestService
    {
        private readonly IDeliveryRequestLineItemRepository _deliveryRequestLineItemRepository;
        private readonly IDeliveryRequestLineRepository _deliveryRequestLineRepository;
        private readonly IDeliveryRequestRepository _deliveryRequestRepository;
        private readonly IDeliveryRequestLineService _deliveryRequestLineService;
        private readonly IItemService _itemService;

        public DeliveryRequestService(IDeliveryRequestRepository deliveryRequestRepository,
            IDeliveryRequestLineRepository deliveryRequestLineRepository,
            IDeliveryRequestLineItemRepository deliveryRequestLineItemRepository,
            IDeliveryRequestLineService deliveryRequestLineService,
            IItemService itemService)
        {
            _deliveryRequestRepository = deliveryRequestRepository;
            _deliveryRequestLineRepository = deliveryRequestLineRepository;
            _deliveryRequestLineItemRepository = deliveryRequestLineItemRepository;
            _deliveryRequestLineService = deliveryRequestLineService;
            _itemService = itemService;
        }

        #region Header

        public long Add(DeliveryRequest obj)
        {
            if (IsDuplicate(obj.DeliveryRequestCode , obj.Id, obj.CustomerId) == false) return _deliveryRequestRepository.Add(obj);
            else return 0;
            
        }

        public IEnumerable<DeliveryRequestLine> GetLinesByDeliveryRequestId(long id)
        {
            return _deliveryRequestLineRepository.GetLinesByDeliveryRequestId(id);
        }

        public IEnumerable<dynamic> GetLinesByDeliveryRequestIdDynamic(long deliveryRequestId)
        {
            var retVal = new List<dynamic>();

            var objs = _deliveryRequestLineRepository.GetLinesByDeliveryRequestId(deliveryRequestId);

            //foreach (var item in _deliveryRequestLineRepository.GetLinesByDeliveryRequestId(deliveryRequestId))
            //    retVal.Add(
            //        new {
            //            item.Id,
            //            item.Product.ProductCode,
            //            ProductDescription = item.Product?.Description??"",
            //            item.Quantity
                        
            //        });

            return objs.Select(obj => new
            {
                Id = obj.Id,
                DeliveryRequestId = obj.DeliveryRequestId,
                CustomerId = obj.CustomerId,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ProductId = obj.ProductId,
                Product = obj.Product?.Description ?? "",
                BrandId = obj.BrandId,
                Brand = obj.Brand?.Name ?? "",
                ItemId = obj.ItemId,
                Item = obj.Item?.Description ?? "",
                ItemCode = obj.Item?.ItemCode ?? "",
                UomId = obj.UomId,
                Uom = obj.Uom?.Description ?? "",
                Quantity = obj.Quantity,
                PickTypeId = obj.PickTypeId,
                PickType = obj.PickType?.Description,
                SpecialInstructions = obj.SpecialInstructions,
                Memo = obj.Memo,
                StatusId = obj.StatusId,
                //Status = obj.Status.Name
            });
        }

      

        public long AddDeliveryRequest(object obj, out string deliveryRequestCode)
        {
            var objModel = JObject.Parse(obj.ToString());
            var deliveryRequest = objModel.ToObject<DeliveryRequest>();
            Func<DeliveryRequest, bool> delRes = x => x.CustomerId == deliveryRequest.CustomerId;
            var count = _deliveryRequestRepository.GetList(delRes).Count();
            deliveryRequestCode = String.Format("DR-{0:0000000000}", count + 1);
            long retId = 0;
            

            if (IsDuplicate(deliveryRequest.DeliveryRequestCode, deliveryRequest.Id, deliveryRequest.CustomerId) == false)
            {
                deliveryRequest.DeliveryRequestCode = deliveryRequestCode;
                retId = _deliveryRequestRepository.Add(deliveryRequest);
            }
            else
            {
                Expression<Func<DeliveryRequest, bool>> res;

                res = x => x.DeliveryRequestCode.ToLower() == deliveryRequest.DeliveryRequestCode.ToLower() && x.CustomerId == deliveryRequest.CustomerId && x.IsActive == false;
                var model = _deliveryRequestRepository.Get(res);

                if(model != null)
                {
                    retId = model.Id;
                    deliveryRequest.Id = retId;
                    deliveryRequest.IsActive = true;

                    _deliveryRequestRepository.Detach(model);

                    _deliveryRequestRepository.Update(deliveryRequest);
                }
            }
            return retId;
        }

        public bool UpdateDeliveryRequest(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var deliveryRequest = objModel.ToObject<DeliveryRequest>();

            if (IsDuplicate(deliveryRequest.DeliveryRequestCode, deliveryRequest.Id, deliveryRequest.CustomerId) == false) return _deliveryRequestRepository.Update(deliveryRequest);
            else return false;
        }

        public dynamic GetByIdDynamic(long id)
        {
            var obj = _deliveryRequestRepository.GetById(id);

            return new
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRef = obj.CustomerRef,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ServiceCode = obj.ServiceCode,
                CustomerClientId = obj.CustomerClientId,
                CustomerClient = obj.CustomerClient?.Name ?? "",
                DeliveryRequestCode = obj.DeliveryRequestCode,
                HaulierId = obj.HaulierId,
                Haulier = obj.Haulier?.Name ?? "",
                RequestType = obj.RequestType,
                SalesOrderRef = obj.SalesOrderRef,
                WarehouseId = obj.WarehouseId,
                Warehouse = obj.Warehouse?.Description ?? "",
                Address = (obj.Warehouse?.Address1 ?? "") + " " + (obj.Warehouse?.Address2 ?? ""),
                StatusId = obj.StatusId,
                Status = obj.Status?.Name ?? "",
                RequestedDate = obj.RequestedDate,
                RequiredDate = obj.RequiredDate,
                RequiredDeliveryDate = obj.RequiredDeliveryDate,
                LatestDate = obj.LatestDate,
                EarliestDate = obj.EarliestDate,
                Priority = obj.Priority,
                IsFullfilled = obj.IsFullfilled
            };
        }

        //public IEnumerable<DeliveryRequest> GetList(int take)
        //{
        //    return _deliveryRequestRepository.GetList(take);
        //}

        public bool Update(DeliveryRequest obj)
        {
            _deliveryRequestRepository.Update(obj);
            return true;
        }

        public bool Delete(long id, string updatedBy)
        {
            var obj = _deliveryRequestRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            return _deliveryRequestRepository.Update(obj);
        }

        public bool Enable(long id, string updatedBy)
        {
            var obj = _deliveryRequestRepository.GetById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            return _deliveryRequestRepository.Update(obj);
        }

        public IEnumerable<DeliveryRequest> GetAll()
        {
            return _deliveryRequestRepository.GetAll();
        }

        public IEnumerable<dynamic> GetByStatusDynamic(string status)
        {
            var retVal = new List<dynamic>();

            var list = _deliveryRequestRepository.GetByStatus(status);
            foreach (var item in list)
                retVal.Add(new
                {
                    item.Id,
                    item.DeliveryRequestCode,
                    item.RequestType,
                    item.RequestedDate,
                    item.RequiredDeliveryDate,
                    item.HaulierId,
                    HaulierName = item.Haulier?.Name ?? "",
                    item.ServiceCode,
                    item.CustomerRef,
                    item.RequiredDate,
                    item.EarliestDate,
                    item.LatestDate,
                    item.SalesOrderRef,
                    WarehouseDescription = item.Warehouse?.Description ?? "",
                    item.Priority,
                    item.IsFullfilled,
                    item.CustomerClientId,
                    CustomerClientName = item.CustomerClient?.Name ?? "",
                    item.DespatchedBy,
                    item.PickedBy,
                    item.Despatched,
                    item.DespatchedDate,
                    item.WarehouseId,
                    item.Warehouse.WarehouseCode,
                    item.StatusId,
                    StatusName = item.Status.Name,
                    item.CustomerId
                });


            return retVal;

        }

        public IEnumerable<DeliveryRequest> GetByStatus(bool isActive, long customerId, string status)
        {
            var retVal = new List<dynamic>();
            Expression<Func<DeliveryRequest, bool>> res;

            res = x => x.Status.Name.ToLower() == status.ToLower() && x.IsActive == isActive
                                                                   && x.CustomerId == customerId;

            //var list = _deliveryRequestRepository.GetList(res);
            return _deliveryRequestRepository.GetList(res);

        }

        public IEnumerable<DeliveryRequest> GetByStatusId(bool isActive, long customerId, long statusId)
        {
            var retVal = new List<dynamic>();
            Expression<Func<DeliveryRequest, bool>> res;

            res = x => x.StatusId == statusId && x.IsActive == isActive
                                                                   && x.CustomerId == customerId;

            //var list = _deliveryRequestRepository.GetList(res);
            return _deliveryRequestRepository.GetList(res);

        }

        public IEnumerable<dynamic> GetByStatusDynamic(bool isActive, long customerId, string status)
        {
            var retVal = new List<dynamic>();
            Expression<Func<DeliveryRequest, bool>> res;

            res = x => x.Status.Name.ToLower() == status.ToLower() && x.IsActive == isActive
                                                                   && x.CustomerId == customerId;

            var objs = _deliveryRequestRepository.GetList(res);
            return objs.Select(obj => new
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRef = obj.CustomerRef,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ServiceCode = obj.ServiceCode,
                CustomerClientId = obj.CustomerClientId,
                CustomerClient = obj.CustomerClient?.Name ?? "",
                DeliveryRequestCode = obj.DeliveryRequestCode,
                HaulierId = obj.HaulierId,
                Haulier = obj.Haulier?.Name ?? "",
                RequestType = obj.RequestType,
                SalesOrderRef = obj.SalesOrderRef,
                WarehouseId = obj.WarehouseId,
                Warehouse = obj.Warehouse?.Description ?? "",
                Address = (obj.Warehouse?.Address1 ?? "") + " " + (obj.Warehouse?.Address2 ?? ""),
                StatusId = obj.StatusId,
                Status = obj.Status?.Name ?? "",
                RequestedDate = obj.RequestedDate,
                RequiredDate = obj.RequiredDate,
                RequiredDeliveryDate = obj.RequiredDeliveryDate,
                LatestDate = obj.LatestDate,
                EarliestDate = obj.EarliestDate,
                Priority = obj.Priority,
                IsFullfilled = obj.IsFullfilled
            });
        }

        public IEnumerable<dynamic> GetByStatusIdDynamic(bool isActive, long customerId, long statusId)
        {
            var retVal = new List<dynamic>();
            //set processing = false - Renz
            var isProcessing = false;
            Expression<Func<DeliveryRequest, bool>> res;

            res = x => x.StatusId == statusId && x.IsActive == isActive
                                                                   && x.CustomerId == customerId;

            var objs = _deliveryRequestRepository.GetList(res);

            return objs.Select(obj => new
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRef = obj.CustomerRef,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ServiceCode = obj.ServiceCode,
                CustomerClientId = obj.CustomerClientId,
                CustomerClient = obj.CustomerClient?.Name ?? "",
                DeliveryRequestCode = obj.DeliveryRequestCode,
                HaulierId = obj.HaulierId,
                Haulier = obj.Haulier?.Name ?? "",
                RequestType = obj.RequestType,
                SalesOrderRef = obj.SalesOrderRef,
                WarehouseId = obj.WarehouseId,
                Warehouse = obj.Warehouse?.Description ?? "",
                Address = (obj.Warehouse?.Address1 ?? "") + " " + (obj.Warehouse?.Address2 ?? ""),
                StatusId = obj.StatusId,
                Status = obj.Status?.Name ?? "",
                RequestedDate = obj.RequestedDate,
                RequiredDate = obj.RequiredDate,
                RequiredDeliveryDate = obj.RequiredDeliveryDate,
                LatestDate = obj.LatestDate,
                EarliestDate = obj.EarliestDate,
                Priority = obj.Priority,
                IsFullfilled = obj.IsFullfilled
            });
        }

        public bool DispatchDeliveryRequest(DeliveryRequestBindingModel obj)
        {
            var deliveryRequest = _deliveryRequestRepository.GetById(obj.Id);

            if (deliveryRequest != null)
            {
                deliveryRequest.DespatchedBy = obj.DespatchedBy;
                deliveryRequest.DespatchedDate = obj.DespatchedDate;
                deliveryRequest.Despatched = true;
                deliveryRequest.UpdatedBy = obj.UpdatedBy;
                deliveryRequest.PickedBy = obj.PickedBy;
                deliveryRequest.StatusId = 15;
                _deliveryRequestRepository.Update(deliveryRequest);
                return true;
            }
            else return false;
        }

        public IEnumerable<dynamic> GetAvailableListDynamic(bool isActive, long customerId, bool isProcessing)
        {
            Expression<Func<DeliveryRequest, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId && x.IsProcessing == isProcessing;
            var objs = _deliveryRequestRepository.GetList(res);

            return objs.Select(obj => new
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRef = obj.CustomerRef,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ServiceCode = obj.ServiceCode,
                CustomerClientId = obj.CustomerClientId,
                CustomerClient = obj.CustomerClient?.Name ?? "",
                DeliveryRequestCode = obj.DeliveryRequestCode,
                HaulierId = obj.HaulierId,
                Haulier = obj.Haulier?.Name ?? "",
                RequestType = obj.RequestType,
                SalesOrderRef = obj.SalesOrderRef,
                WarehouseId = obj.WarehouseId,
                Warehouse = obj.Warehouse?.Description ?? "",
                Address = (obj.Warehouse?.Address1 ?? "") + " " + (obj.Warehouse?.Address2 ?? ""),
                StatusId = obj.StatusId,
                Status = obj.Status?.Name ?? "",
                RequestedDate = obj.RequestedDate,
                RequiredDate = obj.RequiredDate,
                RequiredDeliveryDate = obj.RequiredDeliveryDate,
                LatestDate = obj.LatestDate,
                EarliestDate = obj.EarliestDate,
                Priority = obj.Priority,
                IsFullfilled = obj.IsFullfilled
            });
        }

        #endregion


        #region Lines

        public long AddDeliveryRequestLine(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var deliveryRequestLine = objModel.ToObject<DeliveryRequestLine>();
            deliveryRequestLine.StatusId = 10;
            long retId = 0;

            if (IsDuplicateLine(deliveryRequestLine.Id, deliveryRequestLine.DeliveryRequestId, deliveryRequestLine.ItemId, deliveryRequestLine.CustomerId) == false)
            {
                retId = _deliveryRequestRepository.AddLine(deliveryRequestLine);
                //return retId;
            }
            else
            {
                Expression<Func<DeliveryRequestLine, bool>> res;
                res = x => x.DeliveryRequestId == deliveryRequestLine.DeliveryRequestId && x.ItemId == deliveryRequestLine.ItemId && x.CustomerId == deliveryRequestLine.CustomerId && x.IsActive == false;
                var model = _deliveryRequestLineRepository.Get(res);
                if(model != null)
                {
                    retId = model.Id;
                    deliveryRequestLine.Id = retId;
                    deliveryRequestLine.IsActive = true;

                    _deliveryRequestLineRepository.Detach(model);

                    _deliveryRequestLineRepository.Update(deliveryRequestLine);

                    //return retId;
                }
            }

            if(retId != 0)
            {
                var header = _deliveryRequestRepository.GetById(deliveryRequestLine.DeliveryRequestId);
                if (header.StatusId == 14)
                {
                    header.StatusId = 13;
                    _deliveryRequestRepository.Update(header);
                }
            }

            return retId;
        }

        public bool AddDeliveryRequestLine(List<object> obj)
        {
            var deliveryRequestList = new List<DeliveryRequestLine>();

            foreach (var o in obj)
            {
                var objModel = JObject.Parse(o.ToString());
                var deliveryRequest = objModel.ToObject<DeliveryRequestLine>();
                deliveryRequestList.Add(deliveryRequest);
            }

            return _deliveryRequestRepository.AddLine(deliveryRequestList);
        }

        public dynamic GetDeliveryRequestLineByIdDynamic(long id)
        {
            var obj = _deliveryRequestRepository.GetLineById(id);

            return new
            {
                Id = obj.Id,
                DeliveryRequestId = obj.DeliveryRequestId,
                CustomerId = obj.CustomerId,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ProductId = obj.ProductId,
                Product = obj.Product?.Description ?? "",
                BrandId = obj.BrandId,
                Brand = obj.Brand?.Name ?? "",
                ItemId = obj.ItemId,
                Item = obj.Item?.Description ?? "",
                ItemCode = obj.Item?.ItemCode ?? "",
                ItemLocation = obj.Item?.Location?.Description ?? "",
                UomId = obj.UomId,
                Uom = obj.Uom?.Description ?? "",
                Quantity = obj.Quantity,
                PickTypeId = obj.PickTypeId,
                PickType = obj.PickType?.Description,
                SpecialInstructions = obj.SpecialInstructions,
                Memo = obj.Memo,
                StatusId = obj.DeliveryRequest.StatusId
            };
        }

        public bool AddLine(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());

            var deliveryRequest = new DeliveryRequestLine();
            deliveryRequest = objModel.ToObject<DeliveryRequestLine>();

            _deliveryRequestRepository.AddLine(deliveryRequest);
            return true;
        }

        public bool DeleteDeliveryRequestLine(long id, string updatedBy)
        {
            var obj = _deliveryRequestRepository.GetLineById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            var retVal = _deliveryRequestRepository.UpdateLine(obj);

            if (retVal)
            {
                var header = _deliveryRequestRepository.GetById(obj.DeliveryRequestId);
                Expression<Func<DeliveryRequestLine, bool>> res = x => x.DeliveryRequestId == obj.DeliveryRequestId && x.IsActive == true;

                var lines = _deliveryRequestLineRepository.GetList(res).ToList();
                
                if(header.StatusId == 13 && (lines == null || lines.Count() == 0))
                {
                    header.StatusId = 14;
                    header.UpdatedBy = updatedBy;
                    _deliveryRequestRepository.Update(header);
                }   
            }

            return retVal;

            //return _deliveryRequestRepository.DeleteLine(id);
        }

        public bool EnableDeliveryRequestLine(long id, string updatedBy)
        {
            var obj = _deliveryRequestRepository.GetLineById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            var retVal = _deliveryRequestRepository.UpdateLine(obj);

            if (retVal)
            {
                var header = _deliveryRequestRepository.GetById(obj.DeliveryRequestId);

                Expression<Func<DeliveryRequestLine, bool>> res = x => x.DeliveryRequestId == obj.DeliveryRequestId && x.IsActive == true;
                var lines = _deliveryRequestLineRepository.GetList(res).ToList();

                if(header.StatusId == 14 && (lines != null && lines.Count() == 1))
                {
                    header.StatusId = 13;
                    header.UpdatedBy = updatedBy;
                    _deliveryRequestRepository.Update(header);
                }
            }

            return retVal;

            //return _deliveryRequestRepository.DeleteLine(id);
        }

        public bool UpdateDeliveryRequestLine(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var deliveryRequestLine = objModel.ToObject<DeliveryRequestLine>();

            if (IsDuplicateLine(deliveryRequestLine.Id, deliveryRequestLine.DeliveryRequestId, deliveryRequestLine.ItemId, deliveryRequestLine.CustomerId) == false)
                return _deliveryRequestRepository.UpdateLine(deliveryRequestLine);
            else return false;
        }

        public bool UpdateDeliveryRequestLines(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());

            var deliveryRequestLines = (List<DeliveryRequestLine>)obj;
            //deliveryRequestLine = objModel.ToObject<DeliveryRequestLine>();
            var retVal = false;

            foreach(var line in deliveryRequestLines)
            {
                if (IsDuplicateLine(line.Id, line.DeliveryRequestId, line.ItemId, line.CustomerId))
                {
                    retVal = _deliveryRequestRepository.UpdateLine(line);
                }
            }
            return retVal;
        }

        public bool CheckIsProcessing(long id)
        {
            return _deliveryRequestRepository.GetById(id).IsProcessing;

        }

        public bool BatchUpdateDeliveryRequestLine(List<DeliveryRequestLine> objs)
        {
            foreach (var item in objs)
            {
                var deliveryRequestLine = _deliveryRequestLineRepository.GetById(item.Id);
                deliveryRequestLine.StatusId = 11;
                deliveryRequestLine.UpdatedBy = item.UpdatedBy;
                _deliveryRequestLineRepository.Update(deliveryRequestLine);

            }
            return true;

        }


        public bool Onprocessing(long id, bool isprocessed, string updatedBy)
        {
            var obj = _deliveryRequestRepository.GetById(id);
            if (obj != null)
            {
                obj.IsProcessing = isprocessed;
                obj.UpdatedBy = updatedBy;
                return _deliveryRequestRepository.Update(obj);
            }

            else return false;

        }


        #endregion

        #region Line Items

        public long AddLineItem(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());

            var deliveryRequest = new DeliveryRequestLineItem();
            deliveryRequest = objModel.ToObject<DeliveryRequestLineItem>();

            return _deliveryRequestLineItemRepository.Add(deliveryRequest);
        }

        public bool UpdateLineItem(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());

            var deliveryRequest = new DeliveryRequestLineItem();
            deliveryRequest = objModel.ToObject<DeliveryRequestLineItem>();

            _deliveryRequestLineItemRepository.Update(deliveryRequest);
            return true;
        }

        public bool DeleteLineItem(long id)
        {
            return _deliveryRequestLineItemRepository.Delete(id);
        }

        public DeliveryRequestLineItem GetLineItemById(long id)
        {
            return _deliveryRequestLineItemRepository.GetById(id);
        }

        public IEnumerable<DeliveryRequestLineItem> GetLineItemsByLineId(long lineId)
        {
            return _deliveryRequestLineItemRepository.GetListByLineId(lineId);
        }

        public IEnumerable<dynamic> GetLineItemsDynamicByLineId(long lineId)
        {
            var retVal = new List<dynamic>();

            var list = _deliveryRequestLineItemRepository.GetListByLineId(lineId);
            foreach (var item in list)
                retVal.Add(new
                {
                    item.Id,
                    item.DeliveryRequestLineId,
                    item.Item.ItemCode,
                    ItemDescription = item.Item?.Description ?? "",
                    item.Status,
                    item.ItemId,
                });

            return retVal;
        }

        public bool UpdateLineItems(List<object> obj)
        {
            List<DeliveryRequestLineItem> list = new List<DeliveryRequestLineItem>();
            foreach(var item in obj)
            {
                var objModel = JObject.Parse(item.ToString());
                list.Add(objModel.ToObject<DeliveryRequestLineItem>());
            }

            return _deliveryRequestLineItemRepository.Update(list);
        }

        #endregion

        #region Transaction

        public bool StockAssign(int id)
        {

            var lines = _deliveryRequestLineRepository.GetLinesByDeliveryRequestId(id);

            foreach(var line in lines)
            {

            }


            return true;
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<DeliveryRequest, bool>> res;

            if (id == 0) res = x => x.DeliveryRequestCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.DeliveryRequestCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _deliveryRequestRepository.Get(res) != null;
        }

        public bool IsDuplicateLine(long id, long? deliveryRequestId, long? itemId, long? customerId)
        {
            Expression<Func<DeliveryRequestLine, bool>> res;

            if (id == 0) res = x => x.DeliveryRequestId == deliveryRequestId && x.ItemId == itemId && x.CustomerId == customerId;
            else res = x => x.DeliveryRequestId == deliveryRequestId && x.ItemId == itemId && x.Id != id && x.CustomerId == customerId;

            return _deliveryRequestLineRepository.Get(res) != null;
        }

        public IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long statusId)
        {
            Expression<Func<DeliveryRequest, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId 
                                                            && (statusId != 0 ? x.StatusId == statusId : true);
            var objs = _deliveryRequestRepository.GetList(res);
            
            return objs.Select(obj => new
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRef = obj.CustomerRef,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ServiceCode = obj.ServiceCode,
                CustomerClientId = obj.CustomerClientId,
                CustomerClient = obj.CustomerClient?.Name ?? "",
                DeliveryRequestCode = obj.DeliveryRequestCode,
                HaulierId = obj.HaulierId,
                Haulier = obj.Haulier?.Name ?? "",
                RequestType = obj.RequestType,
                SalesOrderRef = obj.SalesOrderRef,
                WarehouseId = obj.WarehouseId,
                Warehouse = obj.Warehouse?.Description ?? "",
                Address = (obj.Warehouse?.Address1 ?? "") + " " + (obj.Warehouse?.Address2 ?? ""),
                StatusId = obj.StatusId,
                Status = obj.Status?.Name ?? "",
                RequestedDate = obj.RequestedDate,
                RequiredDate = obj.RequiredDate,
                RequiredDeliveryDate = obj.RequiredDeliveryDate,
                LatestDate = obj.LatestDate,
                EarliestDate = obj.EarliestDate,
                Priority = obj.Priority,
                IsFullfilled = obj.IsFullfilled
            });
        }

        public IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long statusId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<DeliveryRequest, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId 
                                                            && (statusId != 0 ? x.StatusId == statusId : true);
            var objs = _deliveryRequestRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);

            return objs.Select(obj => new
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRef = obj.CustomerRef,
                IsActive = obj.IsActive,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                ServiceCode = obj.ServiceCode,
                CustomerClientId = obj.CustomerClientId,
                CustomerClient = obj.CustomerClient?.Name ?? "",
                DeliveryRequestCode = obj.DeliveryRequestCode,
                HaulierId = obj.HaulierId,
                Haulier = obj.Haulier?.Name ?? "",
                RequestType = obj.RequestType,
                SalesOrderRef = obj.SalesOrderRef,
                WarehouseId = obj.WarehouseId,
                Warehouse = obj.Warehouse?.Description ?? "",
                Address = (obj.Warehouse?.Address1 ?? "") + " " + (obj.Warehouse?.Address2 ?? ""),
                StatusId = obj.StatusId,
                Status = obj.Status?.Name ?? "",
                RequestedDate = obj.RequestedDate,
                RequiredDate = obj.RequiredDate,
                RequiredDeliveryDate = obj.RequiredDeliveryDate,
                LatestDate = obj.LatestDate,
                EarliestDate = obj.EarliestDate,
                Priority = obj.Priority,
                IsFullfilled = obj.IsFullfilled
            });
        }

        DeliveryRequest IGenericService<DeliveryRequest>.GetById(long id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<DeliveryRequest> IGenericService<DeliveryRequest>.GetList(bool isActive, long customerId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<DeliveryRequest> IGenericService<DeliveryRequest>.GetList(bool isActive, long customerId, int pageNo, int pageSize)
        {
            throw new NotImplementedException();
        }

        public DeliveryRequestLine GetDeliveryRequestLineById(long id)
        {
            throw new NotImplementedException();
        }







        #endregion
    }
}