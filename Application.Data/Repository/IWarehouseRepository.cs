using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenGts.Model;

namespace CenGts.Data.Repository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Warehouse GetByWarehouseCode(string warehouseCode);
    }
}
