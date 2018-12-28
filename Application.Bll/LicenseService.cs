using Application.Data.Repository;
using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _licenseRepository;
        public LicenseService(ILicenseRepository licenseRepository)
        {
            this._licenseRepository = licenseRepository;
        }

        public long Add(License obj)
        {
            try
            {
                return _licenseRepository.Add(obj);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            //var obj = _licenseRepository.GetById(id);
            //obj.IsActive = false;
            //return _licenseRepository.Update(obj);

            return _licenseRepository.Delete(id);
        }

        public License Get(Expression<Func<License, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<License> GetAll()
        {
            return _licenseRepository.GetAll();
        }

        public License GetById(long id)
        {
            return _licenseRepository.GetById(id);
        }

        //public IEnumerable<License> GetList(int take)
        //{
        //    return _licenseRepository.GetList(take);
        //}

        public IEnumerable<License> GetList(Expression<Func<License, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<License> GetList(bool isActive, long customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<License> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }

        public bool Update(License obj)
        {
            try
            {
                _licenseRepository.Update(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
