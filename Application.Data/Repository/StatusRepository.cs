using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;
using Application.Model.Transaction;

namespace Application.Data.Repository
{
    public class StatusRepository : IStatusRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public long Add(Status obj)
        {
            _db.Statuses.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.Statuses.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<Status> GetAll()
        {
            return _db.Statuses;
        }

        public Status GetById(long id)
        {
            return _db.Statuses.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Status> GetListByTransTypeId(long id)
        {
            return _db.Statuses.Where(x => x.TransactionTypeId == id);
        }

        //public IEnumerable<Status> GetList(int take)
        //{
        //    return _db.Statuses.Take(take);
        //}

        public bool Update(Status obj)
        {
            //var status = GetById(obj.Id);
            //status.Name = obj.Name;

            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;

            _db.Entry(obj).State = EntityState.Modified;
            //_db.S tatuses.Attach(obj);
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

        public Status GetByStatus(string status)
        {
            return _db.Statuses.FirstOrDefault(x => x.Name.ToLower() == status.ToLower());
        }

        public Status Get(Expression<Func<Status, bool>> predicate)
        {
            return _db.Statuses.FirstOrDefault(predicate);
        }

        public IEnumerable<Status> GetList(Expression<Func<Status, bool>> predicate)
        {
            return _db.Statuses.Where(predicate);
        }

        public void Detach(Status obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}