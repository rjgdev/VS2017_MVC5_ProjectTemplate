using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Product : BaseClass
    {
        public long Id { get; set; }

        [Display(Name = "Product")]
        public string Description { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        //public long? CustomerId { get; set; }

        public long? UomId { get; set; }

        //public virtual Customer Customer { get; set; }
        //public virtual ICollection<Brand> Brands { get; set; }
        //public virtual Uom Uom { get; set; }
    }

    public class ProductSelectListViewModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
    }
}
