using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class TransactionTypesViewModel
    {

        public long Id { get; set; }

        public long? CustomerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        [Required(ErrorMessage = "Type Code is required")]
        [Display(Name = "Type Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [Display(Name = "Type")]
        public string TransType { get; set; }

        [Display(Name = "Company Code")]
        public string Domain { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}