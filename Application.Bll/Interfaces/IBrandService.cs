using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Bll
{
    public interface IBrandService : IGenericService<Brand>
    {
        //Brand GetByBrandCode(string code);
        IEnumerable<Brand> GetByProductId(long id);
        IEnumerable<Brand> GetByProductCode(string code, bool isActive, long customerId);
        IEnumerable<Brand> GetByItemCode(string code);
        dynamic GetByBrandCode(string code);
        bool Enable(long id, string updatedBy);
    }
}
