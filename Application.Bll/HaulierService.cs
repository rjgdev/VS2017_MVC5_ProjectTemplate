using System.Collections.Generic;
using Application.Data.Repository.Interfaces;
using Application.Model;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Application.Bll
{
    public class HaulierService : IHaulierService
    {
        private readonly IHaulierRepository _haulierRepository;

        public HaulierService()
        {
        }

        public HaulierService(IHaulierRepository haulierRepository)
        {
            _haulierRepository = haulierRepository;
        }

        public Haulier GetById(long id)
        {
            return _haulierRepository.GetById(id);
        }

        //public IEnumerable<Haulier> GetList(int take)
        //{
        //    return _haulierRepository.GetList(100);
        //}

        public long Add(Haulier obj)
        {
            return _haulierRepository.Add(obj);
        }

        public long Add(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var haulier = objModel.ToObject<Haulier>();
            //(Haulier)obj;

            if (IsDuplicate(haulier.HaulierCode, haulier.Id, haulier.CustomerId) == false) return _haulierRepository.Add(haulier);
            else
            {
                Expression<Func<Haulier, bool>> res = x => x.HaulierCode == haulier.HaulierCode && x.CustomerId == haulier.CustomerId && x.IsActive == false;
                var model = _haulierRepository.Get(res);

                if(model != null)
                {
                    haulier.Id = model.Id;
                    haulier.IsActive = true;

                    _haulierRepository.Detach(model);

                    _haulierRepository.Update(haulier);
                    return haulier.Id;
                }
                else return 0;
            }
        }

        public bool Update(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var haulier = objModel.ToObject<Haulier>();

            if (IsDuplicate(haulier.HaulierCode, haulier.Id,haulier.CustomerId) == false) return _haulierRepository.Update(haulier);
            else return false;
        }

        public bool Update(Haulier obj)
        {
            return _haulierRepository.Update(obj);
        }

        public bool Delete(long id, string updatedBy)
        {
            var obj = _haulierRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            return _haulierRepository.Update(obj);

            //return _haulierRepository.Delete(id);
        }

        public bool Enable(long id,string updatedBy)
        {
            var obj = _haulierRepository.GetById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            return _haulierRepository.Update(obj);

            //return _haulierRepository.Delete(id);
        }

        public IEnumerable<Haulier> GetAll()
        {
            return _haulierRepository.GetAll();
        }

        public Haulier GetByHaulierCode(string code)
        {
            return _haulierRepository.GetByHaulierCode(code);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Haulier, bool>> res;

            if (id == 0) res = x => x.HaulierCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.HaulierCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == x.CustomerId;

            return _haulierRepository.Get(res) != null;
        }

        public IEnumerable<Haulier> GetList(bool isActive, long customerId)
        {
            Expression<Func<Haulier, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _haulierRepository.GetList(res);
        }

        public IEnumerable<Haulier> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Haulier, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _haulierRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}