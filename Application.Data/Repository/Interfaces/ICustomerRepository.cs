using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetByDomain(string domain);
    }
}
