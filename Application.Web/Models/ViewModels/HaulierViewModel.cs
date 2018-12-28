using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class HaulierViewModel
    {
        [Key]
        public long Id { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Haulier Code is required")]
        [Display(Name = "Haulier Code")]
        public string HaulierCode { get; set; }

        [Required(ErrorMessage = "Haulier Name is required")]
        [Display(Name = "Haulier Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        [Display(Name = "Telephone")]
        [RegularExpression(@"^[^.]+$", ErrorMessage = "Please enter a valid number.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Mobile Numbe is required")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Please enter valid mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Url]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

    }
}