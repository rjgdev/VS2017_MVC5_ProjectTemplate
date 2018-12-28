using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class DeliveryRequestLineItemRepository : IDeliveryRequestLineItemRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public DeliveryRequestLineItem GetById(long id)
        {
            return _db.DeliveryRequestLineItems.Find(id);
        }

        public long Add(DeliveryRequestLineItem obj)
        {
            _db.DeliveryRequestLineItems.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.DeliveryRequestLineItems.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public bool Update(DeliveryRequestLineItem obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.DeliveryRequestLineItems.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        //public IEnumerable<DeliveryRequestLineItem> GetList(int take)
        //{
        //    return _db.DeliveryRequestLineItems.Take(take);
        //}

        public IEnumerable<DeliveryRequestLineItem> GetAll()
        {
            return _db.DeliveryRequestLineItems;
        }

        public IEnumerable<DeliveryRequestLineItem> GetListByLineId(long lineId)
        {
            return _db.DeliveryRequestLineItems.Where(w => w.DeliveryRequestLineId == lineId).ToList();
        }

        public IEnumerable<DeliveryRequestLineItem> GetListByDeliveryRequestId(long deliveryRequestId)
        {
            return _db.DeliveryRequestLineItems.Where(
                w => w.DeliveryRequestLine.DeliveryRequest.Id == deliveryRequestId);
        }

        public IEnumerable<DeliveryRequestLineItem> GetListByDeliveryRequestCode(string deliveryRequestCode)
        {
            return _db.DeliveryRequestLineItems.Where(w =>
                w.DeliveryRequestLine.DeliveryRequest.DeliveryRequestCode == deliveryRequestCode);
        }

        public bool Update(List<DeliveryRequestLineItem> list)
        {
            foreach(var item in list)
            {
                _db.Entry(item).State = EntityState.Modified;
            }
            _db.SaveChanges();
            return true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
        }

        public DeliveryRequestLineItem Get(Expression<Func<DeliveryRequestLineItem, bool>> predicate)
        {
            return _db.DeliveryRequestLineItems.FirstOrDefault(predicate);
        }

        public IEnumerable<DeliveryRequestLineItem> GetList(Expression<Func<DeliveryRequestLineItem, bool>> predicate)
        {
            return _db.DeliveryRequestLineItems.Where(predicate);
        }

        public void Detach(DeliveryRequestLineItem obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}