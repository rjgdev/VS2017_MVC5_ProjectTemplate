using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// </summary>
        /// <param name="domain"></param>
        public Customer GetByDomain(string domain)
        {
            return _db.Customers.FirstOrDefault(x => x.Domain == domain);
        }

        public long Add(Customer obj)
        {
            _db.Customers.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Update(Customer obj)
        {
            //var customer = GetByDomain(obj.Domain);

            //customer.CompanyName = obj.CompanyName;
            //customer.EmailAddress = obj.EmailAddress;
            //customer.FirstName = obj.FirstName;
            //customer.LastName = obj.LastName;
            //customer.MiddleName = obj.MiddleName;
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Customers.Attach(obj);
            _db.SaveChanges();

            return true;
        }

        public Customer GetById(long id)
        {
            return _db.Customers.Find(id);
        }

        //public IEnumerable<Customer> GetList(int take)
        //{
        //    return _db.Customers.Take(take);
        //}

        public bool Delete(long id)
        {
            _db.Customers.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _db.Customers;
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

        public Customer Get(Expression<Func<Customer, bool>> predicate)
        {
            return _db.Customers.FirstOrDefault(predicate);
        }

        public IEnumerable<Customer> GetList(Expression<Func<Customer, bool>> predicate)
        {
            return _db.Customers.Where(predicate);
        }

        public void Detach(Customer obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}