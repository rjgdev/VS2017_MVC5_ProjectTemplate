using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model
{
    [Table("CustomerClients")]
    public class CustomerClient : Address
    {

        [Key]
        public long Id { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }

        public string ContactPerson { get; set; }
        public string Telephone { get; set; }
        public string MobileNo { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Url]
        public string Website { get; set; }

        //public ICollection<CustomerClientAddress> CustomerClientAddresses { get; set; }
    }

    //[Table("CustomerClientAddresses")]
    //public class CustomerClientAddress : Address
    //{
    //    [Key]
    //    public long Id { get; set; }

    //    public long CustomerClientId { get; set; }

    //    public virtual CustomerClient CustomerClient { get; set; }
    //}


}