using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Data.Repository.Interfaces;
using Application.Model;

namespace Application.Data.Repository
{
    public class HaulierRepository : IHaulierRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Haulier GetById(long id)
        {
            var vendor = _db.Hauliers.Find(id);
            if (vendor == null)
                throw new NullReferenceException($"No haulier record found for vendor ID [{id}]");

            return vendor;
        }

        //public virtual IEnumerable<Haulier> GetList(int take)
        //{
        //    return _db.Hauliers.Take(take).ToList();
        //}

        public virtual long Add(Haulier obj)
        {
            _db.Hauliers.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public virtual bool Update(Haulier obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Hauliers.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public virtual bool Delete(long id)
        {
            _db.Hauliers.Remove(GetById(id));
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

        public IEnumerable<Haulier> GetAll()
        {
            return _db.Hauliers;
        }

        public Haulier GetByHaulierCode(string code)
        {
            return _db.Hauliers.FirstOrDefault(x => x.HaulierCode.Equals(code));
        }

        public Haulier Get(Expression<Func<Haulier, bool>> predicate)
        {
            return _db.Hauliers.FirstOrDefault(predicate);
        }

        public IEnumerable<Haulier> GetList(Expression<Func<Haulier, bool>> predicate)
        {
            return _db.Hauliers.Where(predicate);
        }

        public void Detach(Haulier obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}