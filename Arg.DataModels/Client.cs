using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("arg.clients")]
    public class Client
    {
        [Key]
        public int clientid { get; set; }

        public string clientname { get; set; }

        public string nickname { get; set; }

        public string countrycode { get; set; }

        public bool hasclientdb { get; set; }

        public string contactname { get; set; }

        public string emailaddress { get; set; }

        public string phonenumber { get; set; }

        public string street1 { get; set; }

        public string street2 { get; set; }

        public string city { get; set; }

        public string statecode { get; set; }

        public string postalcode { get; set; }

        public int countryid { get; set; }

        public bool isactive { get; set; }
    }
}
