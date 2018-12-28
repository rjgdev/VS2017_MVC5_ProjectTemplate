using CenGts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenGts.Data.Repository
{
    public interface IAuditLogRepository
    {
        AuditLog GetAuditLog(int id);

        void AddAuditLog(AuditLog auditLog);

        void DeleteAuditLog(AuditLog auditLog);

        void UpdateAuditLog(AuditLog auditLog);
    }
}
