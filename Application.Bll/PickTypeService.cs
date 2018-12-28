using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Repository;
using Application.Model;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class PickTypeService : IPickTypeService
    {
        private readonly IPickTypeRepository _pickTypeRepository;

        public PickTypeService(IPickTypeRepository pickTypeRepository)
        {
            _pickTypeRepository = pickTypeRepository;
        }

        public long Add(PickType obj)
        {
            if (IsDuplicate(obj.Code, obj.Id, obj.CustomerId) == false) return _pickTypeRepository.Add(obj);
            else
            {
                Expression<Func<PickType, bool>> res = x => x.Code == obj.Code && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var model = _pickTypeRepository.Get(res);

                if (model != null)
                {
                    obj.Id = model.Id;
                    obj.IsActive = true;

                    _pickTypeRepository.Detach(model);

                    _pickTypeRepository.Update(obj);
                    return obj.Id;
                }
                else return 0;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            var pickType = _pickTypeRepository.GetById(id);
            pickType.IsActive = false;
            pickType.UpdatedBy = updatedBy;
            return _pickTypeRepository.Update(pickType);
        }

        public bool Enable(long id, string updatedBy)
        {
            var pickType = _pickTypeRepository.GetById(id);
            pickType.IsActive = true;
            pickType.UpdatedBy = updatedBy;
            return _pickTypeRepository.Update(pickType);
        }

        public IEnumerable<PickType> GetAll()
        {
            return _pickTypeRepository.GetAll();
        }

        public PickType GetById(long id)
        {
            return _pickTypeRepository.GetById(id);
        }

        //public IEnumerable<PickType> GetList(int take)
        //{
        //    return _pickTypeRepository.GetList(take);
        //}

        public IEnumerable<PickType> GetList(bool isActive, long customerId)
        {
            Expression<Func<PickType, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _pickTypeRepository.GetList(res);
        }

        public IEnumerable<PickType> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<PickType, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _pickTypeRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<PickType, bool>> res;

            if (id == 0) res = x => x.Code.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.Code.ToLower() == code.ToLower() && x.CustomerId == customerId && x.Id != id;

            return _pickTypeRepository.Get(res) != null;
        }

        public bool Update(PickType obj)
        {
            if (IsDuplicate(obj.Code, obj.Id, obj.CustomerId) == false) return _pickTypeRepository.Update(obj);
            else return false;
            
        }
    }
}
