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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService (ICustomerRepository customerRepository )
        {
            this._customerRepository = customerRepository;
        }

        public long Add(Customer customer)
        {
            //Customer customer = new Customer
            //{
            //    CompanyName = CompanyName,
            //    LastName = LastName,
            //    FirstName = FirstName,
            //    MiddleName = MiddleName,
            //    EmailAddress = Email,

            //};
            long retId = 0;
            if (IsDuplicate(customer.EmailAddress, customer.Id) == false) return _customerRepository.Add(customer);
            else
            {
                Expression<Func<Customer, bool>> res = x => x.EmailAddress.ToLower() == customer.EmailAddress.ToLower() && x.IsActive == false;
              
                var retCustomer = _customerRepository.Get(res);
                if (retCustomer != null)
                {

                    customer.Id = retCustomer.Id;
                    customer.IsActive = true;

                    _customerRepository.Detach(retCustomer);
                    _customerRepository.Update(customer);
                    retId = customer.Id;
                }

            }
            return retId;
        }

        public Customer GetByDomain(string domain)
        {
            return _customerRepository.GetByDomain(domain);
        }

        public Customer GetById(long id)
        {
            return _customerRepository.GetById(id);
        }

        public bool Delete(long id, string updatedBy)
        {
            var customer= _customerRepository.GetById(id);
            customer.IsActive = false;
            customer.UpdatedBy = updatedBy;
            return _customerRepository.Update(customer);
        }

        public bool Enable(long id, string updatedBy)
        {
            var customer = _customerRepository.GetById(id);
            customer.IsActive = true;
            customer.UpdatedBy = updatedBy; 
            return _customerRepository.Update(customer);
        }

        public bool Update(Customer obj)
        {
            if (IsDuplicate(obj.EmailAddress, obj.Id) == false) return _customerRepository.Update(obj);
            else return false;

          
        }

        //public IEnumerable<Customer> GetList(int take)
        //{
        //    return _customerRepository.GetList(take);
        //}

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id)
        {
            Expression<Func<Customer, bool>> res;

            if (id == 0) res = x => x.EmailAddress.ToLower() == code.ToLower();
            else res = x => x.EmailAddress.ToLower() == code.ToLower() && x.Id != id;

            return _customerRepository.Get(res) != null;
        }

        public IEnumerable<Customer> GetList(bool isActive)
        {
            Expression<Func<Customer, bool>> res = x => x.IsActive == isActive;
            return _customerRepository.GetList(res);
        }


        public IEnumerable<Customer> GetList(bool isActive, long customerId)
        {
            Expression<Func<Customer, bool>> res = x => x.IsActive == isActive && x.Id == customerId;
            return _customerRepository.GetList(res);
        }

        public IEnumerable<Customer> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Customer, bool>> res = x => x.IsActive == isActive;
            return _customerRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }
    }
}
