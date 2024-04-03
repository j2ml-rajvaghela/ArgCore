using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("BOLHeaders")]
    public class BOLHeaders
    {
        public string BranchCode { get; set; }
        public string BookingReference { get; set; }
        public string JobNumber { get; set; }
        public string ConsignmentID { get; set; }
        public string ServiceID { get; set; }
        public string Vessel1 { get; set; }
        public string Vessel2 { get; set; }
        public string Voyage1 { get; set; }
        public string Voyage2 { get; set; }
        public int ChargeableWeight { get; set; }
        public decimal FreightRevenue { get; set; }
        public decimal FreightCost { get; set; }
        public decimal OtherRevenue { get; set; }

        public decimal TotalRevenue
        {
            get
            {
                return FreightRevenue + OtherRevenue;
            }
        }

        public string Direction { get; set; }
        public int GrossWeight { get; set; }
        public int Pieces { get; set; }
        public string ServiceMovementTypeCode { get; set; }
        public string ServiceLevelCode { get; set; }
        public string ServiceTypeCode { get; set; }
        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }
        public string PortOfExitCode { get; set; }
        public string PortofEntryCode { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("BOL#")]
        public string BOL { get; set; }

        public DateTime? JobConfirmationDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? JobCreatedDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? loaded { get; set; }
        public DateTime? LoadDate { get; set; }
        public string ShipperID { get; set; }
        public string ConsigneeID { get; set; }
        public string NotifyPartyCode { get; set; }
        public string ExportingCarrierCode { get; set; }
        public string PrepaidCollectCode { get; set; }

        [Computed]
        public int Idx { get; set; }

        [Computed]
        public int ResultCount { get; set; }

        [Computed]
        public string Origin { get; set; }

        [Computed]
        public string Destination { get; set; }

        [Computed]
        public string Shipper { get; set; }

        [Computed]
        public string Consignee { get; set; }

        [Computed]
        public string ServiceMovementType { get; set; }

        [Computed]
        public string ServiceLevel { get; set; }

        [Computed]
        public string ServiceType { get; set; }

        [Computed]
        public string OriginLocation { get; set; }

        [Computed]
        public string PortOfExit { get; set; }

        [Computed]
        public string PortofEntry { get; set; }

        [Computed]
        public string NotifyParty { get; set; }

        [Computed]
        public string ExportingCarrier { get; set; }

        [Computed]
        public string PrepaidCollect { get; set; }

        [Computed]
        public string BOLViewed { get; set; }

        [Computed]
        public string ParticipantName { get; set; }

        [Computed]
        public int ShipmentCount { get; set; }

        [Computed]
        public decimal MinCharges { get; set; }

        [Computed]
        public decimal MaxCharges { get; set; }

        [Computed]
        public decimal Difference { get; set; }

        [Computed]
        public decimal StandardDeviation { get; set; }
    }
}
