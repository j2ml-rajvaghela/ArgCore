using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("Participants")]
    public class Participants
    {
        [Key]
        public string ParticipantID { get; set; }
        public string ParticipantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Attention { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string DateAdded { get; set; }
        public string Type { get; set; }
        public string ShipperFlag { get; set; }
        public string ForwarderFlag { get; set; }
        public string CHBR { get; set; }
        public string ConsigneeFlag { get; set; }
        public string ENDFlag { get; set; }
        public string CARRFlag { get; set; }
        public string PayorFlag { get; set; }
        public string CustomerRollerup { get; set; }
        public string MarketSegment { get; set; }
        public string SalesOWner { get; set; }
        public DateTime? Uploaded { get; set; }
    }
}
