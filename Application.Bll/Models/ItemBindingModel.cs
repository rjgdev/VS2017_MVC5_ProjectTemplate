using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Bll.Models
{
    public class ItemBindingModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }

        public long? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }

        public long? WarehouseId  { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseDescription { get; set; }

        public long? LocationId { get; set; }
        public string LocationCode { get; set; }

        public long? CustomerId { get; set; }

        public long? UomId { get; set; }

        public long? BrandId { get; set; }
        public string BrandCode { get; set; }
        
        public string ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

    }
}