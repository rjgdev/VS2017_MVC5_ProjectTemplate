using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bll
{
    public interface ICustomerClientService : IGenericService<CustomerClient>
    {
        bool Enable(long id, string UpdateBy);
        bool Update(CustomerClient obj, out bool duplicate);
    }
}
