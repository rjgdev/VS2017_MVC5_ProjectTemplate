using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Application.Web.Models.ViewModels
{
    public class ItemViewModel
    {
        public long Id { get; set; }

        public long? CustomerId { get; set; }

        public string BatchCode { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public long? LocationId { get; set; }

        public long? WarehouseId { get; set; }

        public long? ProductId { get; set; }

        [Required(ErrorMessage = "Item Description is required")]
        [Display(Name = "Item Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        [Display(Name = "Brand")]
        public long? BrandId { get; set; }

        [Display(Name = "Brand")]
        public string BrandCode { get; set; }

        [Display(Name = "Brand")]
        public string BrandName { get; set; }

        [Display(Name = "Item Group")]
        public string ProductDesc { get; set; }

        [Required(ErrorMessage = "Item Code is required")]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required(ErrorMessage = "Product Code is required")]
        [Display(Name = "Item Group")]
        public string ProductCode { get; set; }

        [Display(Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Received Date")]
        public DateTime? ReceivedDate { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedBy { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value should not be less than 0")]
        [Display(Name = "Quantity")]
        public long? Quantity { get; set; }

        [Display(Name = "Location Code")]
        public string LocationCode { get; set; }

        [Display(Name = "Location")]
        public string LocationDesc { get; set; }

        [Required(ErrorMessage = "Warehouse is required")]
        [Display(Name = "Warehouse")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Warehouse")]
        public string WarehouseDesc { get; set; }

        public string Status { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }


        public string GoodsReceivedNumber { get; set; }

        public string StatusCode { get; set; }

        public int ExpectedReceiptId { get; set; }

        public int ExpectedReceiptLineId { get; set; }
    }
}