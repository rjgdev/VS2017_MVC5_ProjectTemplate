using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class CustomerClientRepository : ICustomerClientRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public CustomerClient GetById(long id)
        {
            return _db.CustomerClients.Find(id);
        }

        public long Add(CustomerClient obj)
        {
            _db.CustomerClients.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.CustomerClients.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public bool Update(CustomerClient obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.CustomerClients.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        //public IEnumerable<CustomerClient> GetList(int take)
        //{
        //    return _db.CustomerClients.Take(take);
        //}

        public IEnumerable<CustomerClient> GetAll()
        {
            return _db.CustomerClients;
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

        public CustomerClient Get(Expression<Func<CustomerClient, bool>> predicate)
        {
            return _db.CustomerClients.FirstOrDefault(predicate);
        }

        public IEnumerable<CustomerClient> GetList(Expression<Func<CustomerClient, bool>> predicate)
        {
            return _db.CustomerClients.Where(predicate);
        }

        public void Detach(CustomerClient obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}