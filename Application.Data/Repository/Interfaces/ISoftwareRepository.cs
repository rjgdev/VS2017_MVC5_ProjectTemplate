using CenGts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenGts.Data.Repository
{
    public interface ISoftwareRepository
    {
        Software GetSoftware(int id);

        void AddSoftware(Software software);

        void DeleteSoftware(Software software);

        void UpdateSoftware(Software software);
    }
}
