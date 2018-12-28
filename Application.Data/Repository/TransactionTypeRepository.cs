using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class TransactionTypeRepository : ITransactionTypeRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TransactionType GetById(long id)
        {
            return _db.TransactionTypes.FirstOrDefault(x => x.Id == id);
        }

        //public IEnumerable<TransactionType> GetList(int take)
        //{
        //    return _db.TransactionTypes.Take(take).ToList();
        //}

        public long Add(TransactionType obj)
        {
            _db.TransactionTypes.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Update(TransactionType obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.TransactionTypes.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            _db.TransactionTypes.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<TransactionType> GetAll()
        {
            return _db.TransactionTypes;
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

        public TransactionType Get(Expression<Func<TransactionType, bool>> predicate)
        {
            return _db.TransactionTypes.FirstOrDefault(predicate);
        }

        public IEnumerable<TransactionType> GetList(Expression<Func<TransactionType, bool>> predicate)
        {
            return _db.TransactionTypes.Where(predicate);
        }

        public void Detach(TransactionType obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}