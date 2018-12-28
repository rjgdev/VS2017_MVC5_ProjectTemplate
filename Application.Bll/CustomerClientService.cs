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
    public class CustomerClientService : ICustomerClientService
    {
        private readonly ICustomerClientRepository _customerClientRepository;

        public CustomerClientService(ICustomerClientRepository customerClientRepository)
        {
            this._customerClientRepository = customerClientRepository;
        }

        public long Add(CustomerClient obj)
        {
            if (IsDuplicate(obj.CustomerCode, obj.Id, obj.CustomerId) == false)   return _customerClientRepository.Add(obj);
            else
            {
                Expression<Func<CustomerClient, bool>> res;

                res = x => x.CustomerCode.ToLower() == obj.CustomerCode.ToLower() && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var customerClient = _customerClientRepository.Get(res);
                if (customerClient != null)
                {
                    obj.Id = customerClient.Id;
                    obj.IsActive = true;

                    _customerClientRepository.Detach(customerClient);

                    _customerClientRepository.Update(obj);
                    return obj.Id;
                }
                else
                {
                    return 0;
                }


            }

        }

        public bool Delete(long id, string updatedBy)
        {
            var obj = _customerClientRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            return _customerClientRepository.Update(obj);


            //return _customerClientRepository.Delete(id);
        }

        public bool Enable(long id, string updatedBy)
        {
            var obj = _customerClientRepository.GetById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            return _customerClientRepository.Update(obj);


            //return _customerClientRepository.Delete(id);
        }

        public IEnumerable<CustomerClient> GetAll()
        {
            return _customerClientRepository.GetAll();
        }

        public CustomerClient GetById(long id)
        {
            return _customerClientRepository.GetById(id);
        }

        //public IEnumerable<CustomerClient> GetList(int take)
        //{
        //    return _customerClientRepository.GetList(take);
        //}

        public IEnumerable<CustomerClient> GetList(bool isActive, long customerId)
        {
            Expression<Func<CustomerClient, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _customerClientRepository.GetList(res);
        }

        public IEnumerable<CustomerClient> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<CustomerClient, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _customerClientRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<CustomerClient, bool>> res;

            if (id == 0) res = x => x.CustomerCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.CustomerCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _customerClientRepository.Get(res) != null;


        }

        public bool Update(CustomerClient obj)
        {
            return _customerClientRepository.Update(obj);

        }

        public bool Update(CustomerClient obj, out bool duplicate)
        {
            if (!IsDuplicate(obj.CustomerCode, obj.Id, obj.CustomerId))
            {
                duplicate = false;
                return _customerClientRepository.Update(obj);
            }
            else 
            {
                duplicate = true;
                return false;
            }


        }
    }
}
