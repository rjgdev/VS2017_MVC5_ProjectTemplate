using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenGts.Model;
using CenGts.Data.Models;
using System.Data.Entity;

namespace CenGts.Data.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Event GetById(int id)
        {
            return _db.Events.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Add(Event obj)
        {
            try
            {
                _db.Events.Add(obj);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                _db.Events.Remove(GetById(id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(Event obj)
        {
            try
            {
                var e = GetById(obj.Id);

                e.EventCode = obj.EventCode;
                e.Affected = obj.Affected;
                e.Cause = obj.Cause;
                e.Trigger = obj.Trigger;
                e.Severity = obj.Severity;

                _db.Entry(e).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetList(int take)
        {
            throw new NotImplementedException();
        }
    }
}
