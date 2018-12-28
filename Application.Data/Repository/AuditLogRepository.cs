using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenGts.Model;
using CenGts.Data.Models;
using System.Data.Entity;
using CenGts.Data.Repository;

namespace CenGts.Data.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void AddAuditLog(AuditLog auditLog)
        {
            _db.AuditLogs.Add(auditLog);
            _db.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditLog"></param>
        public void DeleteAuditLog(AuditLog auditLog)
        {
            _db.AuditLogs.Remove(auditLog);
            _db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AuditLog for specific id</returns>
        public AuditLog GetAuditLog(int id)
        {
            return _db.AuditLogs.Find(id);
        }

        public void UpdateAuditLog(AuditLog auditLog)
        {
            _db.Entry(auditLog).State = EntityState.Modified;
        }
    }
}
