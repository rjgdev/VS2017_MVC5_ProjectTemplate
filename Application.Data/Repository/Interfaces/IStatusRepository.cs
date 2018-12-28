using Application.Model;
using System.Collections.Generic;

namespace Application.Data.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStatusRepository : IRepository<Status>
    {
        Status GetByStatus(string status);
        IEnumerable<Status> GetListByTransTypeId(long id);
    }
}
