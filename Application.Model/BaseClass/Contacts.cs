using System.ComponentModel.DataAnnotations;

namespace Application.Model
{
    public class Contacts : BaseClass
    {
        public string ContactPerson { get; set; }
        public string Telephone { get; set; }
        public string MobileNo { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Url]
        public string Website { get; set; }
    }
}