using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public interface IEventLogRepository : IRepository<EventLog>
    {
        IEnumerable<EventLog> GetList(int page, int pageSize);
        IEnumerable<EventLog> GetList(string logType, int page, int pageSize);
        IEnumerable<EventLog> GetListByDateRange(DateTime dateFrom, DateTime dateTo, string logType, int page, int pageSize);
    }
}
