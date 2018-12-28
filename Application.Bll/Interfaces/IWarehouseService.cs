using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Bll.Models;

namespace Application.Bll
{
    public interface IWarehouseService : IGenericService<Warehouse>
    {
        //Warehouse GetById(long id);
        //IEnumerable<Warehouse> GetList(int take);
        //bool Add(Warehouse obj);
        //bool Add(WarehouseBindingModel obj);
        //bool Update(Warehouse obj);
        bool Enable(long id, string updatedBy );
        //long Add(WarehouseBindingModel obj);
        //bool Update(WarehouseBindingModel obj);
        Warehouse GetByWarehouseCode(string warehouseCode);
    }
}
