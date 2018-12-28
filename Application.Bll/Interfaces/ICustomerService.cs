using System.Collections.Generic;
using Application.Model;

namespace Application.Bll
{
    public interface ICustomerService : IGenericService<Customer>
    {
        //void Add(Customer customer);
        bool Enable(long id, string updatedBy);
        //void Edit(int id);
        //void Update(Customer customer);
        //Customer GetById(long id);
        bool IsDuplicate(string code, long id);
        IEnumerable<Customer> GetList(bool isActive);
        Customer GetByDomain(string domain);

    }
}