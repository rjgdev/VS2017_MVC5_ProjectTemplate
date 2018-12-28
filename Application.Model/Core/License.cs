using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class License
    {
        public long Id { get; set; }
        public string LicenseKey { get; set; }
        public int SoftwareId { get; set; }
        public DateTime LicenseCreated { get; set; }
        public DateTime LicenseExpiry { get; set; }
        public bool NeverExpire { get; set; }

    }
}
