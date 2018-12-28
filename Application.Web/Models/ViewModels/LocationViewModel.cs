using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class LocationViewModel : BaseModel
    {
        public long Id { get; set; }

        [Display(Name = "Location Code")]
        public string LocationCode { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Warehouse is required")]
        [Display(Name = "Warehouse")]
        public long WarehouseId { get; set; }

        [Display(Name = "Warehouse Code")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Warehouse")]
        public string WarehouseDescription { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be not less than 1")]
        [Required(ErrorMessage = "Order is required")]
        [Display(Name = "Order")]
        public int? Order { get; set; }

        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Description == null)
            {
                yield return new EnhancedMappedValidationResult<LocationViewModel>(d => d.Description, "Warehouse Description is required");

            }
            if (Order == null || Order == 0)
            {
                yield return new EnhancedMappedValidationResult<LocationViewModel>(d => d.Order, "Order is required");

            }

        }
    }



}