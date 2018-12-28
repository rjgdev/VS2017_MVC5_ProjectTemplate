using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        public long? CustomerId { get; set; }

        public long? UomId { get; set; }

        [Display(Name = "Item Group")]
        [Required(ErrorMessage = "Item Group is required")]
        public string Description { get; set; }

        [Display(Name = "Item Group Code")]
        [Required(ErrorMessage = "Item Group Code is required")]
        public string ProductCode { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

    }
}