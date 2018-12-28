using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Api.Models
{
    public class ItemBindingModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ItemCode { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }

        public string LocationCode { get; set; }
        public string WarehouseCode { get; set; }

    }
}