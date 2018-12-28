using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bll.Models
{
    public class ExpectedReceiptLineBindingModel
    {
        public long Id { get; set; }

        public long ExpectedReceiptId { get; set; }

        public int Line { get; set; }

        public string UomDescription { get; set; }

        public int? ProductId { get; set; }

        public string ProductCode { get; set; }

        public int? ItemId { get; set; }

        public int? UomId { get; set; }

        public int Quantity { get; set; }

        public long? BrandId { get; set; }

        public string Batch { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public string Comments { get; set; }

        public string Image { get; set; }

        //public string LocationCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string GoodsReceivedNumber { get; set; }

        public long? CustomerId { get; set; }

        public bool IsItemExist { get; set; }

        public bool IsActive { get; set; }

        public bool IsChecked { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public long StatusId { get; set; }
    }
}
