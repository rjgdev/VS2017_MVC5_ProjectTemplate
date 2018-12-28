using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Repository;
using Application.Model;

namespace Application.Bll
{
    public interface IPickTypeService : IGenericService<PickType>
    {
        bool Enable(long id, string updatedBy);
    }
}
