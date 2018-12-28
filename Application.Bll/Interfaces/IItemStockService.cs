using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bll.Models;
using Application.Model;

namespace Application.Bll
{
    public interface IItemStockService : IGenericService<ItemStock>
    {
        bool Enable(long id, string updatedBy);
    }
}
