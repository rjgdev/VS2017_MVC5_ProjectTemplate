using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{

    public class DeliveryRequestViewModel
    {
        [Key]
        public long Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        //[Required(ErrorMessage = "Request Code is required")]
        [Display(Name = "Delivery Request Code")]
        public string DeliveryRequestCode { get; set; }

        [Required(ErrorMessage = "Request Type is required")]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; }

        [Required(ErrorMessage = "RequestedDate is required")]
        [Display(Name = "Requested Date")]
        public DateTime RequestedDate { get; set; }

        [Display(Name = "Required Delivery Date")]
        public DateTime? RequiredDeliveryDate { get; set; }

        [Required(ErrorMessage = "Haulier is required")]
        [Display(Name = "Haulier")]
        public long? HaulierId { get; set; }

        [Display(Name = "Haulier")]
        public string HaulierName { get; set; }

        [Display(Name = "Service Code Number")]
        public string ServiceCode { get; set; }

        [Display(Name = "Customer Reference")]
        public string CustomerRef { get; set; }

        [Display(Name = "Required Date")]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "Earliest Date")]
        public DateTime? EarliestDate { get; set; }

        [Display(Name = "Latest Date")]
        public DateTime? LatestDate { get; set; }

        [Display(Name = "Sales Order Reference")]
        public string SalesOrderRef { get; set; }

        [Required(ErrorMessage = "Warehouse is required")]
        [Display(Name = "Warehouse")]
        public long? WarehouseId { get; set; }

        [Display(Name = "Warehouse")]
        public string WarehouseDescription { get; set; }

        [Display(Name = "Warehouse Address")]
        public string Address { get; set; }

        [Display(Name = "Customer")]
        public long CustomerId { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        [Required(ErrorMessage = "Priority is Required")]
        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [Display(Name = "Is Fullfilled")]
        public bool IsFullfilled { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Client is Required")]
        [Display(Name = "Client")]
        public long? CustomerClientId { get; set; }

        [Display(Name = "Client Name")]
        public string CustomerClientName { get; set; }


        public long? StatusId { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public bool IsProcessing { get; set; }
    }


    public class DeliveryRequestLineViewModel
    {
        [Key]
        public long Id { get; set; }

        public int CustomerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        [Required(ErrorMessage = "Line Number is required")]
        [Display(Name = "Line Number")]
        public int? LineNumber { get; set; }


        [Display(Name = "Delivery Request")]
        public long DeliveryRequestId { get; set; }

        [Required(ErrorMessage = "Item Group is required")]
        [Display(Name = "Item Group")]
        public long? ProductId { get; set; }


        [Display(Name = "Item Group")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Item is required")]
        [Display(Name = "Item")]
        public long? ItemId { get; set; }

        [Display(Name = "Item")]
        public string ItemDescription { get; set; }

        [Required(ErrorMessage = "Pick Type is required")]
        [Display(Name = "Pick Type")]
        public long? PickTypeId { get; set; }

        [Display(Name = "Pick Type")]
        public string PickType { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be not less than 1")]
        [Display(Name = "Quantity")]
        public int? Quantity { get; set; }

        [Display(Name = "Special Instructions")]
        public string SpecialInstructions { get; set; }

        [Display(Name = "Memo")]
        public string Memo { get; set; }

        //[Display(Name = "Delivery Request Code")]
        //public string DeliveryRequestCode { get; set; }

        [Display(Name = "Brand")]
        public int BrandId { get; set; }

        public string Brand { get; set; }

        [Display(Name = "Uom")]
        public long? UomId { get; set; }

        [Display(Name = "Uom")]
        public string UomDescription { get; set; }

        [Display(Name ="Status")]
        public long? StatusId { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }


    public class DeliveryRequestLineItemViewModel
    {
        [Key]
        public int Id { get; set; }

        public long? DeliveryRequestId { get; set; }

        [Required(ErrorMessage = "Delivery Request Line is required")]
        [Display(Name = "Delivery Request Line")]
        public long? DeliveryRequestLineId { get; set; } //TODO: change to DeliverRequestLineId

        [Required(ErrorMessage = "Item is required")]
        [Display(Name = "Item")]
        public long ItemId { get; set; }

        [Display(Name = "Item")]
        public string ItemDescription { get; set; }

        public long? ProductId { get; set; }
    }
}