using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bll.Models
{
    public class WarehouseBindingModel
    {
        public long Id { get; set; }
        public string WarehouseCode { get; set; }

        public string Description { get; set; }

        public string Domain { get; set; }
        public string AddressCode { get; set; }
    }
}
