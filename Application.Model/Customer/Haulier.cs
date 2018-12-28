using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("Hauliers")]
    public class Haulier : Contacts
    {
        [Key]
        public long Id { get; set; }

        public string HaulierCode { get; set; }
        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
    }
}