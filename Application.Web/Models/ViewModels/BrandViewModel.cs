using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class BrandViewModel : BaseModel
    {
        public long Id { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Brand Code is required")]
        [Display(Name = "Brand Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Brand Name is required")]
        [Display(Name = "Brand Name")]
        public string Name { get; set; }

        [Display(Name = "Transaction Type")]
        public long TransactionTypeId { get; set; }

        [Display(Name = "Transaction Type")]
        public string TrasactionTypeDescription { get; set; }

        [Display(Name = "Item Group")]
        public string ProductCode { get; set; }

        [Display(Name = "Item Group")]
        public int? ProductId { get; set; }

        [Display(Name = "Item Group")]
        public string ProductDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Code == null)
            {
                yield return new EnhancedMappedValidationResult<BrandViewModel>(d => d.Code, "Code is required");

            }
            if (Name == null)
            {
                yield return new EnhancedMappedValidationResult<BrandViewModel>(d => d.Name, "Name is required");

            }
        }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }


    
}