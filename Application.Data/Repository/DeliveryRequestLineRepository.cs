using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class DeliveryRequestLineRepository : IDeliveryRequestLineRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public DeliveryRequestLine GetById(long id)
        {
            return _db.DeliveryRequestLines.FirstOrDefault(x => x.Id == id);
        }

        public long Add(DeliveryRequestLine obj)
        {
            _db.DeliveryRequestLines.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.DeliveryRequestLines.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public bool Update(DeliveryRequestLine obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.DeliveryRequestLines.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<DeliveryRequestLine> GetLinesByDeliveryRequestId(long id)
        {
            return _db.DeliveryRequestLines.Where(w => w.DeliveryRequestId == id).ToList();
        }

        //public IEnumerable<DeliveryRequestLine> GetList(int take)
        //{
        //    return _db.DeliveryRequestLines.Take(take);
        //}

        public IEnumerable<DeliveryRequestLine> GetAll()
        {
            return _db.DeliveryRequestLines;
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

        public DeliveryRequestLine Get(Expression<Func<DeliveryRequestLine, bool>> predicate)
        {
            return _db.DeliveryRequestLines.FirstOrDefault(predicate);
        }

        public IEnumerable<DeliveryRequestLine> GetList(Expression<Func<DeliveryRequestLine, bool>> predicate)
        {
            return _db.DeliveryRequestLines.Where(predicate);
        }

        public void Detach(DeliveryRequestLine obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}