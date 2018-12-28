using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Repository;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class UomService : IUomService
    {
        private readonly IUomRepository _uomRepository;

        public UomService(IUomRepository uomRepository)
        {
            this._uomRepository = uomRepository;
        }

        public long Add(Uom obj)
        {
            if (IsDuplicate(obj.Code, obj.Id, obj.CustomerId) == false) return _uomRepository.Add(obj);
            else
            {
                Expression<Func<Uom, bool>> res = x => x.Code == obj.Code && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var model = _uomRepository.Get(res);

                if(model != null)
                {
                    obj.Id = model.Id;
                    obj.IsActive = true;

                    _uomRepository.Detach(model);

                    _uomRepository.Update(obj);
                    return obj.Id;
                }
                else return 0;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            var customerDelete = _uomRepository.GetById(id);
            customerDelete.IsActive = false;
            customerDelete.UpdatedBy = updatedBy;
            return _uomRepository.Update(customerDelete);
        }

        public bool Enable(long id, string updatedBy)
        {
            var customerDelete = _uomRepository.GetById(id);
            customerDelete.IsActive = true;
            customerDelete.UpdatedBy = updatedBy;
            return _uomRepository.Update(customerDelete);
        }

        public IEnumerable<UomSelectListViewModel> GetSelectList()
        {
            return _uomRepository.GetSelectList();
        }

        public Uom GetById(long id)
        {
            return _uomRepository.GetById(id);
        }

        //public IEnumerable<Uom> GetList(int take)
        //{
        //    return _uomRepository.GetList(take);
        //}

        public bool Update(Uom obj)
        {
            if (IsDuplicate(obj.Code, obj.Id, obj.CustomerId) == false) return _uomRepository.Update(obj);
            else return false;
        }

        public IEnumerable<Uom> GetAll()
        {
            return _uomRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Uom, bool>> res;

            if (id == 0) res = x => x.Code.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.Code.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _uomRepository.Get(res) != null;
        }

        public IEnumerable<Uom> GetList(bool isActive, long customerId)
        {
            Expression<Func<Uom, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _uomRepository.GetList(res);
        }

        public IEnumerable<Uom> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Uom, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _uomRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}
