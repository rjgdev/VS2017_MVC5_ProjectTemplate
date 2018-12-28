using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class UomViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Uom Code is Required")]
        [Display(Name = "Uom Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Uom Description is Required")]
        [Display(Name = "Uom Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public virtual CustomerViewModel CustomerViewModel { get; set; }
    }
}