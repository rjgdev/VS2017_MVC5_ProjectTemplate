using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model.Transaction
{
    [Table("Transactions")]
    public class Transaction : BaseClass
    {
        [Key]
        public long Id { get; set; }

        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }

        //TODO: Add other fields
    }
}