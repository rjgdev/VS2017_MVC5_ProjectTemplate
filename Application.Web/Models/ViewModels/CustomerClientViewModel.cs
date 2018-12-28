using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class CustomerClientViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer Code is required")]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        [Display(Name = "Telephone")]
        [RegularExpression(@"^(\d{9})$", ErrorMessage = "Please enter valid telephone number")]
        //[DataType(DataType.PhoneNumber, ErrorMessage = "Provided phone number not valid")]
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

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}