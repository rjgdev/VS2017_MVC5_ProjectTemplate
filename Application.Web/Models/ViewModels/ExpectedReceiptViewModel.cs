using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class ExpectedReceiptViewModel
    {
        [Table("ExpectedReceipts")]
        public class ExpectedReceipt
        {
            [Key]
            public long? Id { get; set; }

            public bool IsActive { get; set; }

            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Expected Receipt Date is required")]
            [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
            [Display(Name = "Expected Receipt Date")]
            public DateTime ExpectedReceiptDate { get; set; }

            public DateTime? DateCreated { get; set; }

            [Required(ErrorMessage = "Goods Received Number is required")]
            [Display(Name = "Goods Received Number")]
            public string GoodsReceivedNumber { get; set; }

            [Required(ErrorMessage = "Reference Number is required")]
            [RegularExpression(@"[a-zA-Z0-9-//()-,.]+$", ErrorMessage = "Invalid Reference Number. It allows input Alphanumeric and , . / ( ) - * + only")]
            [Display(Name = "Reference Number")]
            public string ReferenceNumber { get; set; }

            [Display(Name = "Reference Number")]
            public string AutoReferenceNumber { get; set; }

            [Required(ErrorMessage = "Received is required")]
            [Display(Name = "Received")]
            public bool Received { get; set; }

            [Display(Name = "Comments")]
            public string Comments { get; set; }

            [Display(Name = "Warehouse Address")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Warehouse is required")]
            [Display(Name = "Warehouse")]
            public string WarehouseCode { get; set; }

            [Display(Name = "Warehouse Description")]
            public string WarehouseDescription { get; set; }

            [Display(Name = "Received By")]
            public string ReceivedBy { get; set; }

            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Received Date is required")]
            [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
            [Display(Name = "Received Date")]
            public DateTime? ReceivedDate { get; set; }

            [Display(Name = "Vendor/Supplier")]
            [RegularExpression(@"[a-zA-Z0-9- //()-,.]+$", ErrorMessage = "Invalid Supplier. It allows input Alphanumeric and , . / ( ) - * + only")]
            public string Supplier { get; set; }

            [Display(Name = "Supplier")]
            public long? VendorId { get; set; }

            [Display(Name = "Haulier Name")]
            public string HaulierName { get; set; }

            [Display(Name = "Haulier Code")]
            public string HaulierCode { get; set; }

            public long? CustomerId { get; set; }

            [Required(ErrorMessage = "Transaction Status is required")]
            public long? StatusId { get; set; }

            public long? HaulierId { get; set; }

            public long? CustomerClientId { get; set; }

            //public int WarehouseId { get; set; }

            [Required(ErrorMessage = "Transaction Status is required")]
            [Display(Name = "Transaction Status")]
            public string StatusCode { get; set; }

            [Display(Name = "Is Planned?")]
            public bool Planned { get; set; }

            public string CreatedBy { get; set; }

            public string UpdatedBy { get; set; }

            public bool IsProcessing { get; set; }
        }

        [Table("ExpectedReceiptLines")]
        public class ExpectedReceiptLine
        {
            [Key]
            public long Id { get; set; }

            public int? CustomerId { get; set; }

            public bool IsActive { get; set; }

            public bool IsItemExist { get; set; }

            public long ExpectedReceiptId { get; set; }

            [Display(Name = "Batch")]
            public string Batch { get; set; }

            [Required]
            public long? ProductId { get; set; }

            [Required]
            public long? UomId { get; set; }

            [Required]
            public long? ItemId { get; set; }

            public long StatusId { get; set; }

            [Required(ErrorMessage = "Line is required")]
            [Display(Name = "Line")]
            public int Line { get; set; }

            [Required(ErrorMessage = "Product Code is required")]
            [Display(Name = "Item Group")]
            public string ProductCode { get; set; }

            [Required(ErrorMessage = "Item Description is required")]
            [Display(Name = "Item Description")]
            public string ItemDescription { get; set; }

            [Required(ErrorMessage = "Item Code is required")]
            [Display(Name = "Item Code")]
            public string ItemCode { get; set; }

            public string BrandCode { get; set; }

            public string Brand { get; set; }

            public long? BrandId { get; set; }

            [Required(ErrorMessage = "Brand is required")]
            [Display(Name = "Brand")]
            public string BrandName { get; set; }

            [Display(Name = "Expiry Date")]
            public DateTime? ExpiryDate { get; set; }

            [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
            [Required(ErrorMessage = "Quantity is required")]
            [Display(Name = "Quantity")]
            public int Quantity { get; set; }

            [Required(ErrorMessage = "Uom is required")]
            [Display(Name = "Uom")]
            public string UomDescription { get; set; }

            public string ReferenceNumber { get; set; }

            public string GoodsReceivedNumber { get; set; }

            public bool IsChecked { get; set; }

            public string StatusCode { get; set; }

            public string CreatedBy { get; set; }

            public string UpdatedBy { get; set; }

            public DateTime? DateCreated { get; set; }

            public DateTime? DateUpdated { get; set; }

        }
    }
}