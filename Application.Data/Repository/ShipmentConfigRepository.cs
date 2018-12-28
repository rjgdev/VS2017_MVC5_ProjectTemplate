using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model.Transaction;

namespace Application.Data.Repository
{
    public class ShipmentConfigRepository : IShipmentConfigRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ShipmentConfig GetById(long id)
        {
            var obj = _db.ShipmentConfigs.Find(id);
            if (obj == null)
                throw new NullReferenceException($"No shipment config record found for ID [{id}]");

            return obj;
        }

        //public virtual IEnumerable<ShipmentConfig> GetList(int take)
        //{
        //    return _db.ShipmentConfigs.Take(take).ToList();
        //}

        public virtual long Add(ShipmentConfig obj)
        {
            _db.ShipmentConfigs.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public virtual bool Update(ShipmentConfig obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.ShipmentConfigs.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public virtual bool Delete(long id)
        {
            _db.ShipmentConfigs.Remove(GetById(id));
            _db.SaveChanges();
            return true;
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

        public IEnumerable<ShipmentConfig> GetAll()
        {
            return _db.ShipmentConfigs;
        }

        public ShipmentConfig Get(Expression<Func<ShipmentConfig, bool>> predicate)
        {
            return _db.ShipmentConfigs.FirstOrDefault(predicate);
        }

        public IEnumerable<ShipmentConfig> GetList(Expression<Func<ShipmentConfig, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Detach(ShipmentConfig obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}