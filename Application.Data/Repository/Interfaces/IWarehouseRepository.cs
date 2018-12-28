using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Data.Repository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Warehouse GetByWarehouseCode(string warehouseCode);
    }
}
