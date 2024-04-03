using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("ShipmentTrackingDetails")]
    public class ShipmentTrackingDetails
    {
        public string BranchCode { get; set; }
        public string BookingReference { get; set; }
        public string JobNumber { get; set; }
        public string ConsignmentID { get; set; }
        public string Direction { get; set; }
        public string ConsolMaster { get; set; }
        public DateTime? JobCreatedDate { get; set; }
        public DateTime? JobConfirmationDate { get; set; }
        public string ServiceLevel { get; set; }
        public string ServiceType { get; set; }
        public string ServiceMovementType { get; set; }
        public string CarrierCode { get; set; }
        public string PortOfExit { get; set; }
        public string PortOfEntry { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string UnitNumber { get; set; }
        public string LoadingReference { get; set; }
        public DateTime? LoadDate { get; set; }
        public string LoadTime { get; set; }
        public DateTime? ExpectedPROG { get; set; }
        public DateTime? ActualPROG { get; set; }
        public string PROGLocation { get; set; }
        public DateTime? ExpectedDeparture { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public string DepartureBranch { get; set; }
        public DateTime? ExpectedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
        public string DeliveryLocation { get; set; }
        public DateTime? loaded { get; set; }
        public string MD5 { get; set; }
        public string File_Name { get; set; }
    }
}
