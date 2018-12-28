using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using System.Linq.Expressions;

namespace Application.Data.Repository
{
    /// <summary>
    /// Location Repository Interface
    /// </summary>
    public interface ILocationRepository : IRepository<Location>
    {
        Location GetByLocationCode(string code);
        IEnumerable<Location> GetListByWarehouseId(long id);
        //IEnumerable<Location> GetList(Expression<Func<Location, bool>> predicate);
        //IEnumerable<Location> GetAll();
    }
}
