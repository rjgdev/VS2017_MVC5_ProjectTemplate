using Application.Data.Repository;
using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class DeliveryRequestLineItemService : IDeliveryRequestLineItemService
    {
        private readonly IDeliveryRequestLineItemRepository _deliveryRequestLineItemRepository;
        public DeliveryRequestLineItemService(IDeliveryRequestLineItemRepository deliveryRequestLineItemRepository)
        {
            this._deliveryRequestLineItemRepository = deliveryRequestLineItemRepository;
        }

        public long Add(DeliveryRequestLineItem obj)
        {
            try
            {
                _deliveryRequestLineItemRepository.Add(obj);
                return obj.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            return _deliveryRequestLineItemRepository.Delete(id);
        }

        public IEnumerable<DeliveryRequestLineItem> GetAll()
        {
            return _deliveryRequestLineItemRepository.GetAll();
        }

        public DeliveryRequestLineItem GetById(long id)
        {
            return _deliveryRequestLineItemRepository.GetById(id);
        }

        //public IEnumerable<DeliveryRequestLineItem> GetList(int take)
        //{
        //    return _deliveryRequestLineItemRepository.GetList(take);
        //}

        public bool Update(DeliveryRequestLineItem obj)
        {
            try
            {
                _deliveryRequestLineItemRepository.Update(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<dynamic> GetLineItemsByDeliveryRequestLineId(long deliveryRequestLineId)
        {
            var retVal = new List<dynamic>();

            var list = _deliveryRequestLineItemRepository.GetListByLineId(deliveryRequestLineId);
            foreach (var item in list)
                retVal.Add(new
                {
                    item.Item.Product.ProductCode,
                    ProductDescription = item.Item.Product.Description,
                    item.Item.ItemCode,
                    ItemDescription = item.Item.Description,
                });

            return retVal;
        }

        public IEnumerable<dynamic> GetLineItemsByDeliveryRequestId(long deliveryRequestId)
        {
            var retVal = new List<dynamic>();

            var list = _deliveryRequestLineItemRepository.GetListByDeliveryRequestId(deliveryRequestId);
            foreach (var item in list)
                retVal.Add(new
                {
                    LineId = item.Id,
                    item.DeliveryRequestLine.Product.ProductCode,
                    LineQuantity = item.DeliveryRequestLine.Quantity,
                    item.Item.ItemCode,
                    item.Item.Description,
                });

            return retVal;
        }

        public IEnumerable<dynamic> GetLineItemsByDeliveryRequestCode(string deliveryRequestCode)
        {
            var retVal = new List<dynamic>();

            var list = _deliveryRequestLineItemRepository.GetListByDeliveryRequestCode(deliveryRequestCode);
            foreach (var item in list)
                retVal.Add(new
                {
                    LineId = item.Id,
                    item.DeliveryRequestLine.Product.ProductCode,
                    LineQuantity = item.DeliveryRequestLine.Quantity,
                    item.Item.ItemCode,
                    item.Item.Description,
                });

            return retVal;
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeliveryRequestLineItem> GetList(bool isActive, long customerId)
        {
            Expression<Func<DeliveryRequestLineItem, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _deliveryRequestLineItemRepository.GetList(res);
        }

        public IEnumerable<DeliveryRequestLineItem> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<DeliveryRequestLineItem, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _deliveryRequestLineItemRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}
