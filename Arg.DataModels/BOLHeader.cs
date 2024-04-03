using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("BOLHeader")]
    public class BOLHeader
    {
        [Column("Bol#")]
        [Key]
        public string BOLNo { get; set; }
        public string BusinessUnit { get; set; }
        public string BookingID { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string OriginLocationCode { get; set; }
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
        public string DestinationLocationCode { get; set; }
        public string Mode { get; set; }
        public string BillType { get; set; }
        public string InvoicePrinted { get; set; }
        public string ShipperID { get; set; }
        public string ConsigneeID { get; set; }
        public string NotifyID { get; set; }
        public string PayorID { get; set; }
        public DateTime? Uploaded { get; set; }

        [Computed]
        public int Idx { get; set; }

        [Computed]
        public string Port { get; set; }

        [Computed]
        public string Shipper { get; set; }

        [Computed]
        public string Consignee { get; set; }

        [Computed]
        public string Payor { get; set; }

        [Computed]
        public string POLCode { get; set; }

        [Computed]
        public string PODCode { get; set; }

        [Computed]
        public string Origin { get; set; }

        [Computed]
        public string Destination { get; set; }

        [Computed]
        public string ParticipantName { get; set; }

        [Computed]
        public string CommodityCode { get; set; }

        [Computed]
        public int ResultCount { get; set; }

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

        [Computed]
        public DateTime? BeginDepartureDate { get; set; }

        [Computed]
        public DateTime? EndDepartureDate { get; set; }

        [Computed]
        public string CommodityDescription { get; set; }

        [Computed]
        public string ContainerSize { get; set; }

        [Computed]
        public string ContainerType { get; set; }

        [Computed]
        public string BOLCharges { get; set; }

        [Computed]
        public string OceanCharges { get; set; }

        [Computed]
        public string BOL { get; set; }

        [Computed]
        public string CBF { get; set; }

        [Computed]
        public DateTime EventDatetime { get; set; }

        [Computed]
        public string BOLViewed { get; set; }

        [Computed]
        public DateTime DischargeDate { get; set; }

        [Computed]
        public string PerDiemCharges { get; set; }

        [Computed]
        public DateTime ReturnDate { get; set; }

        [Computed]
        public TimeSpan DifferenceDate
        {
            get
            {
                return ReturnDate - DischargeDate;
            }
        }
    }
}

