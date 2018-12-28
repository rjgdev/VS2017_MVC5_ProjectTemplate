using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenGts.Data.Repository;
using CenGts.Model;

namespace CenGts.Bll
{
    public class CustomerProfileService
    {
        private readonly ICustomerProfileRepository _customerProfileRepository;

        public CustomerProfileService (ICustomerProfileRepository customerProfileRepository )
        {
            this._customerProfileRepository = customerProfileRepository;
        }

        public void Add(string CompanyName, string Email)
        {
            Customer customerProfile = new Customer
            {
                CompanyName = CompanyName,
                EmailAddress = Email
            };

            _customerProfileRepository.AddCustomerProfile(customerProfile);
        }
    }
}
