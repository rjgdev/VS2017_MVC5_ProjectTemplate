using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class TransactionStatusViewModel
    {
        public long Id { get; set; }

        public int CustomerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        [Required(ErrorMessage = "Status Code is required")]
        [Display(Name = "Status Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Status Name is required")]
        [Display(Name = "Status Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Transaction Type is required")]
        [Display(Name = "Transaction Type")]
        public long TransactionTypeId { get; set; }

        [Display(Name = "Transaction Code")]
        public string TransTypeCode { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransTypeDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

    }
}