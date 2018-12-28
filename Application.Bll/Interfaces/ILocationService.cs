using System.Collections.Generic;
using Application.Model;
using System.Linq.Expressions;
using System;

namespace Application.Bll
{
    public interface ILocationService : IGenericService<Location>
    {
        dynamic GetLocationById(int id);
        //IEnumerable<dynamic> GetDynamicList(int take);
        Location GetByLocationCode(string code);
        IEnumerable<Location> GetByWarehouseCode(string warehouseCode, bool isActive, long customerId);
        //IEnumerable<Location> GetAll();
        bool Enable(long id, string updatedBy);

    }
}