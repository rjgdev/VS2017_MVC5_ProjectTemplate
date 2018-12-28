using System.ComponentModel.DataAnnotations;

namespace Application.Model
{
    public class Location : BaseClass
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Description { get; set; }

        public long WarehouseId { get; set; }

        public int Order { get; set; }

        public string LocationCode { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
}