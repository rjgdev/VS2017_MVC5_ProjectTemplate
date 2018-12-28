using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("TransactionTypes")]
    public class TransactionType : BaseClass
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        public string TransType { get; set; }
    }
}