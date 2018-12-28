using CenGts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenGts.Data.Repository
{
    public interface ISoftwareVersionRepository
    {
        SoftwareVersion GetSoftwareVersion(int id);

        void AddSoftwareVersion(SoftwareVersion softwareVersion);

        void DeleteSoftwareVersion(SoftwareVersion softwareVersion);

        void UpdateSoftwareVersion(SoftwareVersion softwareVersion);
    }
}
