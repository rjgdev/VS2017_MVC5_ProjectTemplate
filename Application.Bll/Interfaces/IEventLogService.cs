using System;
using System.Collections.Generic;
using Application.Model;

namespace Application.Bll
{
    public interface IEventLogService : IGenericService<EventLog>
    {
        IEnumerable<EventLog> GetList(int page, int pageSize);
        IEnumerable<EventLog> GetList(string logType, int page, int pageSize);
        IEnumerable<EventLog> GetListByDateRange(DateTime dateFrom, DateTime dateTo, string logType, int page, int pageSize);
    }
}