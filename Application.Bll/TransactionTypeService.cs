using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bll.Models;
using Application.Data.Repository;
using Application.Model;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ICustomerService _customerService;

        public TransactionTypeService(ITransactionTypeRepository transactionTypeRepository, ICustomerService customerService,
            IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _customerService = customerService;
        }

        public long Add(TransactionType obj)
        {
            return _transactionTypeRepository.Add(obj);
        }

        public long Add(TransactionTypeBindingModel obj)
        {
            var customer = _customerService.GetByDomain(obj.Domain);
            var transType = new TransactionType
            {
                TransType = obj.TransType,
                CustomerId = customer.Id
            };
            return _transactionTypeRepository.Add(transType);
        }

        public long Add(object obj)
        {
            try
            {
                var objModel = JObject.Parse(obj.ToString());
                var transactionType = objModel.ToObject<TransactionType>();

                if (IsDuplicate(transactionType.Code, transactionType.Id, transactionType.CustomerId) == false) return _transactionTypeRepository.Add(transactionType);
                else
                {
                    Expression<Func<TransactionType, bool>> res = x => x.Code == transactionType.Code && x.CustomerId == transactionType.CustomerId && x.IsActive == false;
                    var model = _transactionTypeRepository.Get(res);

                    if(model != null)
                    {
                        transactionType.Id = model.Id;
                        transactionType.IsActive = true;

                        _transactionTypeRepository.Detach(model);

                        _transactionTypeRepository.Update(transactionType);
                        return transactionType.Id;
                    }
                    else return 0;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Update(object obj)
        {
            try
            {
                var objModel = JObject.Parse(obj.ToString());
                var transactionType = objModel.ToObject<TransactionType>();

                if (IsDuplicate(transactionType.Code, transactionType.Id, transactionType.CustomerId) == false) return _transactionTypeRepository.Update(transactionType);
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            var transactionType = _transactionTypeRepository.GetById(id);
            transactionType.IsActive = false;
            transactionType.UpdatedBy = updatedBy;

            var retVal = _transactionTypeRepository.Update(transactionType);

            if (retVal == true)
            {
                var statuses = _statusRepository.GetListByTransTypeId(id).ToList();

                if(statuses != null || statuses.Count() > 0)
                {
                    foreach(var status in statuses)
                    {
                        status.IsActive = false;
                        status.UpdatedBy = updatedBy;
                        _statusRepository.Update(status);
                    }
                }
            }

            return retVal;
        }

        public bool Enable(long id, string updatedBy)
        {
            var transactionType = _transactionTypeRepository.GetById(id);
            transactionType.IsActive = true;
            transactionType.UpdatedBy = updatedBy;
            return _transactionTypeRepository.Update(transactionType);
        }

        public TransactionType GetById(long id)
        {
            return _transactionTypeRepository.GetById(id);
        }

        //public IEnumerable<TransactionType> GetList(int take)
        //{
        //    return _transactionTypeRepository.GetList(take);
        //}

        public bool Update(TransactionType obj)
        {
            return _transactionTypeRepository.Update(obj);
        }

        public IEnumerable<TransactionType> GetAll()
        {
            return _transactionTypeRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<TransactionType, bool>> res;

            if (id == 0) res = x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase);
            else res = x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && x.Id != id && x.CustomerId == customerId;

            return _transactionTypeRepository.Get(res) != null;
        }

        public IEnumerable<TransactionType> GetList(bool isActive, long customerId)
        {
            Expression<Func<TransactionType, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _transactionTypeRepository.GetList(res);
        }

        public IEnumerable<TransactionType> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<TransactionType, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _transactionTypeRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}
