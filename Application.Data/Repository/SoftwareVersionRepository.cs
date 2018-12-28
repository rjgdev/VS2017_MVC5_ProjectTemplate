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
    public class SoftwareVersionRepository : ISoftwareVersionRepository
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void AddSoftwareVersion(SoftwareVersion softwareVersion)
        {
            _db.SoftwareVersions.Add(softwareVersion);
            _db.SaveChanges();
        }

        public void DeleteSoftwareVersion(SoftwareVersion softwareVersion)
        {
            _db.SoftwareVersions.Remove(softwareVersion);
            _db.SaveChanges();
        }

        public SoftwareVersion GetSoftwareVersion(int id)
        {
            return _db.SoftwareVersions.Find(id);
        }

        public void UpdateSoftwareVersion(SoftwareVersion softwareVersion)
        {
            _db.Entry(softwareVersion).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
