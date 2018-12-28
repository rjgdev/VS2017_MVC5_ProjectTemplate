using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Item : BaseClass
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }

        public long? BrandId { get; set; }

        [Display(Name = "Item")]
        public string Description { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Batch Code")]
        public string BatchCode { get; set; }

        [Display(Name = "Received Date")]
        public DateTime? ReceivedDate { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedBy { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Warehouse")]
        public long? WarehouseId { get; set; }

        [Display(Name = "Location")]
        public long? LocationId { get; set; }

        public string Status { get; set; }

        //public long? Qty { get; set; }

        public virtual Location Location { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set;  }
        public virtual Brand Brand { get; set; }
    }

    public class ItemSelectListViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
