using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Customer : IEntity
    {
        
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddressToken { get; set; }
        public string EmailAddressTokenExpiry { get; set; }
        public string Domain { get; set; }
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

    }
}
