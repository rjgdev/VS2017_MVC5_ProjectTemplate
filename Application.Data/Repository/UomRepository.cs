using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class UomRepository : IUomRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Uom GetById(long id)
        {
            return _db.Uoms.Find(id);
        }

        public long Add(Uom obj)
        {
            //var uom = _db.Uoms.FirstOrDefault(x => x.Description.ToLower() == obj.Description.ToLower());
            //if (uom != null)
            //{
            //    return 0;

            //}
            //else
            //{
            //    _db.Uoms.Add(obj);
            //    _db.SaveChanges();
            //    return obj.Id;
            //}

            _db.Uoms.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.Uoms.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public bool Update(Uom obj)
        {
            //var uom = _db.Uoms.FirstOrDefault(x => x.Description.ToLower() == obj.Description.ToLower() && x.Id != obj.Id);
            //if (uom != null)
            //{
            //    return false;

            //}
            //else
            //{
            //    _db.Entry(obj).State = EntityState.Modified;
            //    _db.SaveChanges();
            //    return true;
            //}

            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Uoms.Attach(obj);
            _db.SaveChanges();
            return true;

        }

        //public IEnumerable<Uom> GetList(int take)
        //{
        //    return _db.Uoms.Take(take);
        //}

        public Uom GetByDescription(string desc)
        {
            return _db.Uoms.FirstOrDefault(x => x.Description.Equals(desc));
        }

        public IEnumerable<UomSelectListViewModel> GetSelectList()
        {
            var list = new List<UomSelectListViewModel>();
            foreach (var item in _db.Uoms.OrderBy(o => o.Description))
                list.Add(new UomSelectListViewModel {Id = item.Id, Description = item.Description});
            return list;
        }

        public IEnumerable<Uom> GetAll()
        {
            return _db.Uoms;
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

        public Uom Get(Expression<Func<Uom, bool>> predicate)
        {
            return _db.Uoms.FirstOrDefault(predicate);
        }

        public IEnumerable<Uom> GetList(Expression<Func<Uom, bool>> predicate)
        {
            return _db.Uoms.Where(predicate);
        }

        public void Detach(Uom obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}