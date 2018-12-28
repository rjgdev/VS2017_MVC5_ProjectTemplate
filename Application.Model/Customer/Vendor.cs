using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("Vendors")]
    public class Vendor : Address 
    {
        [Key]
        public long Id { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string Telephone { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
    }
}