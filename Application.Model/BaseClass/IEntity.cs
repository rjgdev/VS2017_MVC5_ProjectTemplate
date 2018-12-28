using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public interface IEntity
    {
        DateTime? DateCreated { get; set; }
        bool IsActive { get; set; }

        DateTime? DateUpdated { get; set; }

        string CreatedBy { get; set; }

        string UpdatedBy { get; set; }
    }
}
