using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Bll
{
    public interface IProductService : IGenericService<Product>
    {
        Product GetByProductCode(string productCode);
        IEnumerable<ProductSelectListViewModel> GetSelectList();
        bool Enable(long id, string updatedBy);
    }
}
