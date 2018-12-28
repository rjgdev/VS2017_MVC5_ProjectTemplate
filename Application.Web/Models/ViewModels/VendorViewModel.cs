using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class VendorViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vendor Code is required")]
        [Display(Name = "Vendor Code")]
        public string VendorCode { get; set; }

        [Required(ErrorMessage = "Vendor Name is required")]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        
        [Display(Name = "Telephone")]
        [RegularExpression(@"^[^.]+$", ErrorMessage = "Please enter a valid number.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Please enter valid mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Website")]
        [Url]
        public string Website { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Postal Code")]
        public string PostCode { get; set; }

        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

    }
}