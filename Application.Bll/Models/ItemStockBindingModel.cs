using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Bll.Models
{
    public class ItemStockBindingModel : ItemStock
    {

        public string Warehousecode { get; set; }
        public string LocationCode { get; set; }
        public string ItemCode { get; set; }
    }
}
