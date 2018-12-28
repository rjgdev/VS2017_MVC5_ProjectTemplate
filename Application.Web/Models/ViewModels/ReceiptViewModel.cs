using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.Web.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }

        public int ExpectedReceiptId { get; set; }

        public int ExpectdReceiptLinesId { get; set; }

        public string Status { get; set; }
    }
}