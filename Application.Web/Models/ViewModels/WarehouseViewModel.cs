using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class WarehouseViewModel
    {
        public long Id { get; set; }

        public long? CustomerId { get; set; }

        [Required(ErrorMessage = "Warehouse Code is required")]
        [Display(Name = "Warehouse Code")]
        public string WarehouseCode { get; set; }

        [Required(ErrorMessage = "Warehouse Name is required")]
        [Display(Name = "Warehouse Name")]
        public string Description { get; set; }

        public string Domain { get; set; }
       
        [Display(Name = "Address Code")]
        public string AddressCode { get; set; }

        [Display(Name = "Address Name")]
        public string AddressName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(13, MinimumLength = 6)]
        [RegularExpression(@"[- +()0-9]+", ErrorMessage = "Invalid Telephone")]
        public string Telephone { get; set; }

        [StringLength(13, MinimumLength = 6)]
        [RegularExpression(@"[- +()0-9]+", ErrorMessage = "Invalid Mobile No")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Url]
        public string Website { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}