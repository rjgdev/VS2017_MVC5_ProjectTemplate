using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("Warehouses")]
    public class Warehouse : Address
    {
        [Key]
        public long Id { get; set; }

        public string Description { get; set; }
        public string WarehouseCode { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}