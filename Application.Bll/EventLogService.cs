using System;
using System.Collections.Generic;
using Application.Data.Repository;
using Application.Model;

namespace Application.Bll
{
    public class EventLogService : IEventLogService
    {
        private readonly IEventLogRepository _eventLogRepository;

        public EventLogService(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }

        public long Add(EventLog obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id, string updatedBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetList(int page, int pageSize)
        {
            return _eventLogRepository.GetList(page, pageSize);
        }
        public IEnumerable<EventLog> GetList(string logType, int page, int pageSize)
        {
            return _eventLogRepository.GetList(logType, page, pageSize);
        }

        public IEnumerable<EventLog> GetListByDateRange(DateTime dateFrom, DateTime dateTo, string logType, int page, int pageSize)
        {
            return _eventLogRepository.GetListByDateRange(dateFrom, dateTo, logType, page, pageSize);
        }

        public EventLog GetById(long id)
        {
            return _eventLogRepository.GetById(id);
        }

        //public IEnumerable<EventLog> GetList(int take)
        //{
        //    return _eventLogRepository.GetList(take);
        //}

        public bool Update(EventLog obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetAll()
        {
            return _eventLogRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetList(bool isActive, long customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}