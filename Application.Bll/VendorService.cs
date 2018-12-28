using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Data.Repository;
using Application.Model;
using Application.Model.Transaction;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Linq;

namespace Application.Bll
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService()
        {
        }

        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public Vendor GetById(long id)
        {
            return _vendorRepository.GetById(id);
        }

        public async Task<Vendor> GetByIdAsync(long id)
        {
            return await _vendorRepository.GetByIdAsync(id);
        }

        //public async Task<IEnumerable<Vendor>> GetListAsync(int take)
        //{
        //    return await _vendorRepository.GetListAsync(100);
        //}

        //public IEnumerable<Vendor> GetList(int take)
        //{
        //    return _vendorRepository.GetList(100);
        //}

        public bool Update(Vendor obj)
        {
            if (IsDuplicate(obj.VendorCode, obj.Id, obj.CustomerId) == false) return _vendorRepository.Update(obj);
            else return false;
        }

        public bool Delete(long id, string updatedBy)
        {
            var vendor = _vendorRepository.GetById(id);
            vendor.IsActive = false;
            vendor.UpdatedBy = updatedBy;
            return _vendorRepository.Update(vendor);
        }

        public bool Enable(long id, string updatedBy)
        {
            var vendor = _vendorRepository.GetById(id);
            vendor.IsActive = true;
            vendor.UpdatedBy = updatedBy;
            return _vendorRepository.Update(vendor);
        }

        public long Add(Vendor obj)
        {
            if (IsDuplicate(obj.VendorCode, obj.Id, obj.CustomerId) == false) return _vendorRepository.Add(obj);
            else
            {
                Expression<Func<Vendor, bool>> res = x => x.VendorCode == obj.VendorCode && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var model = _vendorRepository.Get(res);

                if(model != null)
                {
                    obj.Id = model.Id;
                    obj.IsActive = true;

                    _vendorRepository.Detach(model);

                    _vendorRepository.Update(obj);

                    return obj.Id;
                }
                else return 0;
            }
        }

        public long Add(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var vendor = objModel.ToObject<Vendor>();

            if (IsDuplicate(vendor.VendorCode, vendor.Id, vendor.CustomerId) == false) return _vendorRepository.Add(vendor);
            else
            {
                Expression<Func<Vendor, bool>> res = x => x.VendorCode == vendor.VendorCode && x.CustomerId == vendor.CustomerId && x.IsActive == false;
                var model = _vendorRepository.Get(res);

                if (model != null)
                {
                    vendor.Id = model.Id;
                    vendor.IsActive = true;

                    _vendorRepository.Detach(model);

                    _vendorRepository.Update(vendor);

                    return vendor.Id;
                }
                else return 0;
            }
        }

        public bool Update(object obj)
        {
            try
            {
                var objModel = JObject.Parse(obj.ToString());
                var vendor = objModel.ToObject<Vendor>();

                if (IsDuplicate(vendor.VendorCode, vendor.Id, vendor.CustomerId) == false) return _vendorRepository.Update(vendor);
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<long> AddAsyc(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());

            var vendor = new Vendor();
            vendor = objModel.ToObject<Vendor>();

            return await _vendorRepository.AddAsync(vendor);
        }

        public async Task<bool> UpdateAsync(object obj)
        {
            try
            {
                var objModel = JObject.Parse(obj.ToString());

                var vendor = new Vendor();
                vendor = objModel.ToObject<Vendor>();

                await _vendorRepository.UpdateAsync(vendor);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Vendor> GetAll()
        {
            return _vendorRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Vendor, bool>> res;

            if (id == 0) res = x => x.VendorCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.VendorCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _vendorRepository.Get(res) != null;
        }

        public IEnumerable<Vendor> GetList(bool isActive, long customerId)
        {
            Expression<Func<Vendor, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _vendorRepository.GetList(res);
        }

        public IEnumerable<Vendor> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Vendor, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _vendorRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}