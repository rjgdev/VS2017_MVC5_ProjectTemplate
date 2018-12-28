using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class EventLogRepository : IEventLogRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //public IEnumerable<EventLog> GetList(int take)
        //{
        //    return _db.Logs.Take(take);
        //}

        public long Add(EventLog obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetListByDateRange(DateTime dateFrom, DateTime dateTo, string logType, int page = 0, int pageSize = 10)
        {
            if (page > 0) page--;
            else if (page < 0) page = 0;

            if (pageSize < 0) pageSize = 10;

            var startDate = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day,0,0,0);
            var endDate = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day,23,59,59);

            var list = _db.Logs.Where(w => w.Level==logType.ToUpper() && (w.Date >= startDate && w.Date <= endDate)).ToList();
                
            var totalCount = list.Count();
            //var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return list.Skip(pageSize * page).Take(pageSize).ToList();

        }

        public EventLog GetById(long id)
        {
            return _db.Logs.Find(id);
        }

        public IEnumerable<EventLog> GetList(int page = 0, int pageSize = 10)
        {
            if (page > 0) page--;
            else if (page < 0) page = 0;

            if (pageSize < 0) pageSize = 10;

            var list = _db.Logs.ToList();

            var totalCount = list.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            return list.Skip(pageSize * page).Take(pageSize).ToList();
        }

        public IEnumerable<EventLog> GetList(string logType, int page = 0, int pageSize = 10)
        {
            if (page > 0) page--;
            else if (page < 0) page = 0;

            if (pageSize < 0) pageSize = 10;

            var list = _db.Logs.Where(w => w.Level == logType.ToUpper()).ToList();

            var totalCount = list.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            return list.Skip(pageSize * page).Take(pageSize).ToList();
        }

        public bool Update(EventLog obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetAll()
        {
            return _db.Logs;
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
        }

        public EventLog Get(Expression<Func<EventLog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> GetList(Expression<Func<EventLog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Detach(EventLog obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}