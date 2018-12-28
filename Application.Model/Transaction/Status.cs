using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("Statuses")]
    public class Status : BaseClass
    {
        [Key]
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? TransactionTypeId { get; set; }

        public  virtual TransactionType TransactionType { get; set; }

    }
}