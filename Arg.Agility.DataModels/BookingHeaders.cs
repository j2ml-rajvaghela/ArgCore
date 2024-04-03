using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("BookingHeaders")]
    public class BookingHeaders
    {
        public string BranchCode { get; set; }
        public string BookingReference { get; set; }
        public string JobNumber { get; set; }
        public string ConsignmentID { get; set; }
        public string Direction { get; set; }
        public string ConsolMaster { get; set; }
        public DateTime? JobConfirmationDate { get; set; }
        public DateTime? JobCreatedDate { get; set; }
        public string ServiceLevel { get; set; }
        public string ServiceType { get; set; }
        public string ServiceMovementType { get; set; }
        public string Voyage1 { get; set; }
        public string Voyage2 { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public string Vessel1 { get; set; }
        public string Vessel2 { get; set; }
        public string Vessel3 { get; set; }
        public string ServiceID { get; set; }
        public string PortOfExit { get; set; }
        public string PortOfEntry { get; set; }
        public string PortOfDischarge { get; set; }
        public string ContractNo { get; set; }
        public string DepartureDateString { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? loaded { get; set; }
        public string MD5 { get; set; }
        public string File_Name { get; set; }
    }
}
