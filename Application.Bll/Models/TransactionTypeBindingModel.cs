using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Bll.Models
{
    public class TransactionTypeBindingModel
    {
        public int Id { get; set; }
        public string TransType { get; set; }
        public string Domain { get; set; }
    }
}