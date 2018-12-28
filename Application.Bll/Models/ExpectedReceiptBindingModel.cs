using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Bll.Models
{
    public class ExpectedReceiptBindingModel
    {

        public ExpectedReceiptBindingModel()
        {

        }
        
        public ExpectedReceiptBindingModel(ExpectedReceipt expectedReceipt)
        {
            Id = expectedReceipt.Id;
            ExpectedReceiptDate = expectedReceipt.ExpectedReceiptDate;
            GoodsReceivedNumber = expectedReceipt.GoodsReceivedNumber;
            Received = expectedReceipt.Received;
            Comments = expectedReceipt.Comments;
            Address = expectedReceipt.Address;
            WarehouseCode = expectedReceipt.Warehouse?.WarehouseCode ?? "";
            ReceivedBy = expectedReceipt.ReceivedBy;
            ReceivedDate = expectedReceipt.ReceivedDate;
            Supplier = expectedReceipt.Supplier;
            HaulierName = expectedReceipt.Haulier?.Name ?? "";

            CustomerId = expectedReceipt.CustomerId;
            StatusId = expectedReceipt.StatusId;
            HaulierId = expectedReceipt.HaulierId;
            HaulierCode = expectedReceipt.Haulier.HaulierCode;
            CustomerClientId = expectedReceipt.CustomerClientId;
            CreatedBy = expectedReceipt.CreatedBy;
            UpdatedBy = expectedReceipt.UpdatedBy;
            IsActive = expectedReceipt.IsActive;
            IsProcessing = expectedReceipt.IsProcessing;

        }

        public long? Id { get; set; }
        public DateTime? ExpectedReceiptDate { get; set; }
        public string GoodsReceivedNumber { get; set; }
        public bool Received { get; set; }
        public string Comments { get; set; }
        public string Address { get; set; }

        public string WarehouseCode { get; set; }
        public string HaulierCode { get; set; }
        public string ReceivedBy { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public string Supplier { get; set; }
        public string HaulierName { get; set; }
        public bool Planned { get; set; }

        public string ReferenceNumber { get; set; }

        public long? CustomerId { get; set; }
        public long? StatusId { get; set; }
        public long? HaulierId { get; set; }
        public long? CustomerClientId { get; set; }
        public bool IsProcessing { get; set; }
        public string AutoReferenceNumber { get; set; }

        public virtual Haulier Haulier { get; set; }
        public virtual Status Status { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Customer Customer { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsActive { get; set; }
        //public ICollection<ExpectedReceiptLine> ExpectedReceiptLines { get; set; }
    }
}
