using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Bll.Models
{
    public class DeliveryRequestBindingModel : DeliveryRequest
    {
        public string ItemLocation { get; set; }

    }
}
