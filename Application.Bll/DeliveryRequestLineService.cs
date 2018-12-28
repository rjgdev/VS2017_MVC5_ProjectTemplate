using Application.Data.Repository;
using Application.Data.Repository.Interfaces;
using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class DeliveryRequestLineService : IDeliveryRequestLineService
    {
        private readonly IDeliveryRequestLineRepository _deliveryRequestLineRepository;

        public DeliveryRequestLineService(IDeliveryRequestLineRepository deliveryRequestLineRepository)
        {
            this._deliveryRequestLineRepository = deliveryRequestLineRepository;
        }

        public long Add(DeliveryRequestLine obj)
        {
            try
            {
                return _deliveryRequestLineRepository.Add(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            return _deliveryRequestLineRepository.Delete(id);
        }

        public IEnumerable<DeliveryRequestLine> GetLinesByDeliveryRequestId(int id)
        {
            return _deliveryRequestLineRepository.GetLinesByDeliveryRequestId(id);
        }

        public DeliveryRequestLine GetById(long id)
        {
            return _deliveryRequestLineRepository.GetById(id);
        }

        //public IEnumerable<DeliveryRequestLine> GetList(int take)
        //{
        //    return _deliveryRequestLineRepository.GetList(take);
        //}

        public bool Update(DeliveryRequestLine obj)
        {
            try
            {
                _deliveryRequestLineRepository.Update(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<DeliveryRequestLine> GetAll()
        {
            return _deliveryRequestLineRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long deliveryRequestId)
        {
            Expression<Func<DeliveryRequestLine, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId
                                                                && x.DeliveryRequestId == deliveryRequestId;

            var objs = _deliveryRequestLineRepository.GetList(res);

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
                ItemLocation = obj.Item.Location.Description ?? "",
                StatusId = obj.StatusId,
                Status = obj.Status.Name
            });
        }

        public IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long deliveryRequestId,
            int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<DeliveryRequestLine, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId
                                                           && x.DeliveryRequestId == deliveryRequestId;
            var objs = _deliveryRequestLineRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);

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
                ItemLocation = obj.Item.Location.Description ?? ""
            });
        }

        public IEnumerable<DeliveryRequestLine> GetList(bool isActive, long customerId, long deliveryRequestId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeliveryRequestLine> GetList(bool isActive, long customerId, long deliveryRequestId, int pageNo, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeliveryRequestLine> GetList(bool isActive, long customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeliveryRequestLine> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}
