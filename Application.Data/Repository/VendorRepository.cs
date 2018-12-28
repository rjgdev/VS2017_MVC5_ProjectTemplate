using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class VendorRepository : IVendorRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Vendor GetById(long id)
        {
            var vendor = _db.Vendors.Find(id);
            if (vendor == null)
                throw new NullReferenceException($"No vendor record found for vendor ID [{id}]");

            return vendor;
        }

        //public virtual IEnumerable<Vendor> GetList(int take)
        //{
        //    return _db.Vendors.Take(take).ToList();
        //}

        public virtual long Add(Vendor obj)
        {
            _db.Vendors.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public virtual bool Update(Vendor obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Vendors.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public virtual bool Delete(long id)
        {
            _db.Vendors.Remove(GetById(id));
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

        public IEnumerable<Vendor> GetAll()
        {
            return _db.Vendors;
        }

        public async Task<Vendor> GetByIdAsync(long id)
        {
            var vendor = await _db.Vendors.FindAsync(id);
            if (vendor == null)
                throw new NullReferenceException($"No vendor record found for vendor ID [{id}]");

            return vendor;
        }

        public async Task<IEnumerable<Vendor>> GetListAsync(int take)
        {
            return await _db.Vendors.Take(take).ToListAsync();
        }

        public async Task<long> AddAsync(Vendor obj)
        {
            _db.Vendors.Add(obj);
            await _db.SaveChangesAsync();
            return obj.Id;
        }

        public async Task<bool> UpdateAsync(Vendor obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            _db.Vendors.Remove(GetById(id));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Vendor>> GetAllAsync()
        {
            return await _db.Vendors.ToListAsync();
        }

        public Vendor Get(Expression<Func<Vendor, bool>> predicate)
        {
            return _db.Vendors.FirstOrDefault(predicate);
        }

        public IEnumerable<Vendor> GetList(Expression<Func<Vendor, bool>> predicate)
        {
            return _db.Vendors.Where(predicate);
        }

        public void Detach(Vendor obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}