using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("UnitOfMeasures")]
    public class Uom : BaseClass
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        
    }

    public class UomSelectListViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}