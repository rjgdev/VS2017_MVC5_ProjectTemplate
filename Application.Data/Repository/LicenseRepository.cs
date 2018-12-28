using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Models;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Application.Data.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class LicenseRepository : ILicenseRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public License GetById(long id)
        {
            return _db.Licences.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long Add(License obj)
        {
            try
            {
                _db.Licences.Add(obj);
                _db.SaveChanges();
                return obj.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            try
            {
                _db.Licences.Remove(GetById(id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(License obj)
        {
            try
            {
                var license = GetById(obj.Id);

                license.LicenseKey = obj.LicenseKey;
                license.NeverExpire = obj.NeverExpire;
                //var getObj = GetById(obj.Id);
                //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
                _db.Entry(license).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //public IEnumerable<License> GetList(int take)
        //{
        //    return _db.Licences.Take(take);
        //}

        public IEnumerable<License> GetAll()
        {
            return _db.Licences;
        }

        public License Get(Expression<Func<License, bool>> predicate)
        {
            return _db.Licences.FirstOrDefault(predicate);
        }

        public IEnumerable<License> GetList(Expression<Func<License, bool>> predicate)
        {
            return _db.Licences.Where(predicate);
        }

        public void Detach(License obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}
