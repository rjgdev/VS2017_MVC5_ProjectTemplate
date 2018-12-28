using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models
{
    public class ListFilterViewModels : DataTableViewModel
    {
        [Display(Name = "Is Active:")]
        public bool? IsActive { get; set; }
    }

    public class ListFilterLineViewModels : ListFilterViewModels
    {
        [Display(Name = "Header")]
        public long HeaderId { get; set; }
    }

    public class ExpectedReceiptFilterViewModels : ListFilterViewModels
    {
        public string from { get; set; }

        public string to { get; set; }

        [Display(Name = "Status:")]
        public string StatusCode { get; set; }
    }

    public class DeliveryRequestFilterViewModels : ListFilterViewModels
    {
        [Display(Name = "Status:")]
        public long? StatusId { get; set; }
    }
}