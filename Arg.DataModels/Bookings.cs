using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("Bookings")]
    public class Bookings
    {
        [Key]
        public string BookingID { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime? BookingCreateDate { get; set; }
        public string BookingSource { get; set; }
        public string BookingMadeBy { get; set; }
        public string Service { get; set; }
        public string Direction { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string POL { get; set; }
        public DateTime? ScheduleDepartureDate { get; set; }
        public string ScheduleDepartureTime { get; set; }
        public DateTime? ActualDepartureDate { get; set; }
        public string ActualDepartureTime { get; set; }
        public string POD { get; set; }
        public DateTime? ScheduleArrivalDate { get; set; }
        public string ScheduleArrivalTime { get; set; }
        public DateTime? ActualArrivalDate { get; set; }
        public string ActualArrivalTime { get; set; }
        public string Mode { get; set; }
        public string OriginLocationCode { get; set; }
        public string OriginNanda { get; set; }
        public string DestinationLocationCode { get; set; }
        public string DestinationNanda { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string DestinationAgentPort { get; set; }
        public string PlaceOfReceipt { get; set; }
        public string ShipperID { get; set; }
        public string ConsigneeID { get; set; }
        public string PayorID { get; set; }
        public string BookingContact { get; set; }
        public string BlockStow { get; set; }
        public string RequiredDeliveryDate { get; set; }
        public string QuoteNumber { get; set; }
        public string ShipperReferenceNumber { get; set; }
        public string ForwarderReferenceNumber { get; set; }
        public string ConsigneeReferenceNumber { get; set; }
        public string NotifyPartyReferenceNumber { get; set; }
        public string PayorReferenceNumber { get; set; }
        public decimal? BookingWeight { get; set; }
        public string EquipmentRequiredDate { get; set; }
        public string EquipmentSize { get; set; }
        public string EquipmentType { get; set; }
        public int? QuantityBooked { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityDescription { get; set; }
        public string Tariff { get; set; }
        public string CustomsCode { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? MinTemperature { get; set; }
        public decimal? MaxTemperature { get; set; }
        public decimal? SetTemperature { get; set; }
        public string VentsOpenClosed { get; set; }
        public string TemperaturePlusOrMinus { get; set; }
        public string ReeferFlag { get; set; }
        public string CelsiusOrFahrenheit { get; set; }
        public string HaulageOrSpecialInstructions { get; set; }
        public string UNHazmatCode { get; set; }
        public string SITFlag { get; set; }
        public string InBondFlag { get; set; }
        public string PatternCode { get; set; }
        public int? RowCount { get; set; }
        public DateTime? Uploaded { get; set; }

        [Computed]
        public string Origin { get; set; }

        [Computed]
        public string Destination { get; set; }

        [Computed]
        public string Shipper { get; set; }

        [Computed]
        public string Consignee { get; set; }

        [Computed]
        public string Payor { get; set; }

        [Computed]
        public string Commodity { get; set; }

        [Computed]
        public string Reference { get; set; }

        [Computed]
        public string BOLNo { get; set; }

        [Computed]
        public string ShipperName { get; set; }

        [Computed]
        public string ConsigneeName { get; set; }

        [Computed]
        public string BillType { get; set; }
    }
}
