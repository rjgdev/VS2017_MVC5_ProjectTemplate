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
    public class SoftwareRepository : ISoftwareRepository
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void AddSoftware(Software software)
        {
            _db.Softwares.Add(software);
            _db.SaveChanges();
        }

        public void DeleteSoftware(Software software)
        {
            _db.Softwares.Remove(software);
            _db.SaveChanges();
        }

        public Software GetSoftware(int id)
        {
            return _db.Softwares.Find(id);
        }

        public void UpdateSoftware(Software software)
        {
            _db.Entry(software).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
