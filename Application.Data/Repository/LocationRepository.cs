using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class LocationRepository : ILocationRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Location GetById(long id)
        {
            return _db.Locations.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Location> GetListByWarehouseId(long id)
        {
            return _db.Locations.Where(x => x.WarehouseId == id);
        }

        //public virtual IEnumerable<Location> GetList(int take)
        //{
        //    return _db.Locations.Take(take).ToList();
        //}

        public virtual long Add(Location obj)
        {
            _db.Locations.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public virtual bool Update(Location obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Locations.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public virtual bool Delete(long id)
        {
            _db.Locations.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public Location GetByLocationCode(string code)
        {
            return _db.Locations.FirstOrDefault(x => x.LocationCode.Equals(code));
        }

        public IEnumerable<Location> GetAll()
        {
            return _db.Locations;
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

        public IEnumerable<Location> GetList(Expression<Func<Location, bool>> predicate)
        {
            return _db.Locations.Where(predicate);
        }

        public Location Get(Expression<Func<Location, bool>> predicate)
        {
            return _db.Locations.FirstOrDefault(predicate);
        }

        public void Detach(Location obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}