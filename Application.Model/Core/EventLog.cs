using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("Logs")]
    public class EventLog
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Date/Time")]
        public DateTime Date { get; set; }

        [Display(Name = "Thread")]
        public string Thread { get; set; }

        [Display(Name = "Level")]
        public string Level { get; set; }

        [Display(Name = "Logger")]
        public string Logger { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Exception Details")]
        public string Exception { get; set; }
    }
}