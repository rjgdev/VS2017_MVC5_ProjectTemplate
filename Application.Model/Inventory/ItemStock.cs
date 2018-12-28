using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class ItemStock : BaseClass
    {
        public long Id { get; set; }
        public int Qty { get; set; }
        public long ItemId { get; set; }
        public long? WarehouseId { get; set; }
        public long? LocationId { get; set; }
        public long? PickTypeId { get; set; }
        public long? VendorId { get; set; }
        public long? ExpectedReceiptLineId { get; set; }
        public long? DeliveryRequestLineId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        

        public virtual Item Item { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Location Location { get; set; }
        public virtual PickType PickType { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual DeliveryRequestLine DeliveryRequestLine { get; set; }
        public virtual ExpectedReceiptLine ExpectedReceiptLine { get; set; }
    }
}
