using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class StockAssignViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }


        [Display(Name = "Product")]
        public int ProductDescription { get; set; }

        [Required(ErrorMessage = "Product is required")]
        [Display(Name = "Item")]
        public int ItemId { get; set; }


        [Display(Name = "Item")]
        public int ItemDescription { get; set; }

        [Required(ErrorMessage = "Warehouse is required")]
        [Display(Name = "Warehouse")]
        public int WarehouseId { get; set; }


        [Display(Name = "Warehouse")]
        public int WarehouseDescription { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Location")]
        public int LocationDescription { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be not less than 1")]
        [Required(ErrorMessage = "Total Stock Quantity is required")]
        [Display(Name = "Total Stock Quantity")]
        public int TotalQtyStock { get; set; }


    }
}