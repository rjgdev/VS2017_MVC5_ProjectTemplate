using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Brand : BaseClass
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }

        public long? ProductId { get; set; }
        public virtual Product Product { get; set; }
        //public virtual ICollection<Product> Products { get; set; }

    }
}
