using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Model.Transaction;

namespace Application.Model
{
    [Table("ExpectedReceipts")]
    public class ExpectedReceipt : BaseClass
    {
        [Key]
        public long Id { get; set; }

        public DateTime? ExpectedReceiptDate { get; set; }

        public string GoodsReceivedNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string AutoReferenceNumber { get; set; }

        public bool Planned { get; set; }

        public bool Received { get; set; }

        public string Comments { get; set; }

        public string Address { get; set; }

        public long? CustomerClientId { get; set; }

        public string Supplier { get; set; }

        public string Courier { get; set; }

        public string ReceivedBy { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public long WarehouseId { get; set; }

        //public long CustomerId { get; set; }

        public long? StatusId { get; set; }

        public long? HaulierId { get; set; }

        public bool IsProcessing { get; set; }

        public virtual Haulier Haulier { get; set; }

        public virtual Status Status { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        //public virtual Customer Customer { get; set; }

        public ICollection<ExpectedReceiptLine> ExpectedReceiptLines { get; set; }
    }

    [Table("ExpectedReceiptLines")]
    public class ExpectedReceiptLine : BaseClass
    {
        [Key]
        public long Id { get; set; }

        public long ExpectedReceiptId { get; set; }

        public string Batch { get; set; }

        public int Line { get; set; }

        public long? ProductId { get; set; }

        public int Quantity { get; set; }

        public long? BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public string ItemCode { get; set; }

        public long? ItemId { get; set; }

        public string ItemDescription { get; set; }

        public virtual Item Item { get; set; }

        public long? UomId { get; set; }

        public string Comments { get; set; }

        public string Image { get; set; }

        public bool IsChecked { get; set; }

        public bool IsItemExist { get; set; }

        public long? VendorId { get; set; }

        public long? StatusId { get; set; }

        public virtual Status Status { get; set; }

        public virtual Vendor Vendor { get; set; }

        public virtual Uom Uom { get; set; }

        public virtual Product Product { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public virtual ExpectedReceipt ExpectedReceipt { get; set; }
    }
}