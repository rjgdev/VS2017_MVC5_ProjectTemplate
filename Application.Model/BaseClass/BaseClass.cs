using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{


    public class BaseClass : IEntity /*: IBaseClasses*/
    {
        public long? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        //public int StatusId { get; set; }
        //public virtual Status Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }


        //public BaseClass()
        //{
        //    // Use private setter in the constructor.
        //    this.DateUpdated = DateTime.Now;
        //}

    }
}
