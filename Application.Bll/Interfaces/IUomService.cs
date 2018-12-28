using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bll
{
    public interface IUomService : IGenericService<Uom>
    {
        IEnumerable<UomSelectListViewModel> GetSelectList();
        bool Enable(long id, string udatedBy);
    }
}
