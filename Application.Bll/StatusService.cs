using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Repository;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public StatusService(IStatusRepository statusRepository,
            ITransactionTypeRepository transactionTypeRepository)
        {
            _statusRepository = statusRepository;
            _transactionTypeRepository = transactionTypeRepository;
        }
        public long Add(Status obj)
        {
            return _statusRepository.Add(obj);
        }

        public bool Delete(long id, string updatedBy)
        {
            var status = _statusRepository.GetById(id);
            status.IsActive = false;
            status.UpdatedBy = updatedBy;
            return _statusRepository.Update(status);
        }

        public bool Enable(long id, string updatedBy)
        {
            var status = _statusRepository.GetById(id);
            status.IsActive = true;
            status.UpdatedBy = updatedBy;
            return _statusRepository.Update(status);
        }

        public IEnumerable<Status> GetAll()
        {
            return _statusRepository.GetAll();
        }

        public Status GetById(long id)
        {
            return _statusRepository.GetById(id);
        }

        public bool Update(Status obj)
        {
            return _statusRepository.Update(obj);
        }

        public long Add(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var status = objModel.ToObject<Status>();

            if (IsDuplicate(status.Code, status.Id, status.Id) == false) return _statusRepository.Add(status);
            else
            {
                Expression<Func<Status, bool>> res = x => x.Code == status.Code && x.CustomerId == status.CustomerId && x.IsActive == false;
                var model = _statusRepository.Get(res);

                if (model != null)
                {
                    status.Id = model.Id;
                    status.IsActive = true;

                    _statusRepository.Detach(model);

                    _statusRepository.Update(status);
                    return status.Id;
                }
                else return 0;
            }
        }

        public bool Update(object obj)
        {
            var objModel = JObject.Parse(obj.ToString());
            var status = objModel.ToObject<Status>();

            if (IsDuplicate(status.Code, status.Id, status.Id) == false) return _statusRepository.Update(status);
            else return false;
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Status, bool>> res;

            if (id == 0) res = x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase);
            else res = x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && x.Id != id && x.CustomerId == customerId;

            return _statusRepository.Get(res) != null;
        }

        public IEnumerable<Status> GetList(bool isActive, long customerId)
        {
            Expression<Func<Status, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _statusRepository.GetList(res);
        }

        public IEnumerable<Status> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Status, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _statusRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}
