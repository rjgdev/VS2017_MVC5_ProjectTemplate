using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenGts.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Customer
    {
        
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddressToken { get; set; }
        public string EmailAddressTokenExpiry { get; set; }
        public string Domain { get; set; }

    }
}
