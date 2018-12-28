using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class WarehouseRepository : IWarehouseRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public long Add(Warehouse obj)
        {
            _db.Warehouses.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.Warehouses.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _db.Warehouses;
        }

        public Warehouse GetById(long id)
        {
            return _db.Warehouses.Find(id);
        }

        public Warehouse GetByWarehouseCode(string warehouseCode)
        {
            return _db.Warehouses.FirstOrDefault(x => x.WarehouseCode.Equals(warehouseCode));
        }

        //public IEnumerable<Warehouse> GetList(int take)
        //{
        //    return _db.Warehouses.Take(take);
        //}

        public bool Update(Warehouse obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Warehouses.Attach(obj);
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

        public Warehouse Get(Expression<Func<Warehouse, bool>> predicate)
        {
            return _db.Warehouses.FirstOrDefault(predicate);
        }

        public IEnumerable<Warehouse> GetList(Expression<Func<Warehouse, bool>> predicate)
        {
            return _db.Warehouses.Where(predicate);
        }

        public void Detach(Warehouse obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}