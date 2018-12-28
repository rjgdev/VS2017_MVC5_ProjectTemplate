using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Application.Web.Models.ViewModels
{
    public class DispatchViewModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int ProductId { get; set; }

        public int WarehouseId { get; set; }

        public int DeliveryRequestId { get; set; }

        [ForeignKey("ItemId")]
        public virtual ItemViewModel Item { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductViewModel Product { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual WarehouseViewModel Warehouse { get; set; }

        [ForeignKey("DeliveryRequestId")]
        public virtual DeliveryRequestViewModel DeliveryRequestViewModel { get; set; }




    }
}