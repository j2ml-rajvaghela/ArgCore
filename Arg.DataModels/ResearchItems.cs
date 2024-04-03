using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("ResearchItems")]
    public class ResearchItems
    {
        [Dapper.Contrib.Extensions.Key]
        public int ResearchId { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string BOL { get; set; }

        [Required(ErrorMessage = "The Booking field is required")]
        public string BookingId { get; set; }

        [Required(ErrorMessage = "The Bol Execution Date field is required")]
        public DateTime BolExecutionDate { get; set; }

        //[Required(ErrorMessage = "The Shipper Ref. Number field is required")]
        //public int ShipperRefNumber { get; set; }
        //[Required(ErrorMessage = "The Forwarder Ref. Number field is required")]
        //public int ForwarderRefNumber { get; set; }
        //[Required(ErrorMessage = "The Transaction Ref. Number field is required")]
        //public int TransactionRefNumber { get; set; }
        //[Required(ErrorMessage = "The Tariff Ref. Number field is required")]
        //public int TariffRefNumber { get; set; }
        //[Required(ErrorMessage = "The Purchase Order Number field is required")]
        //public int PurchaseOrderNumber { get; set; }
        //[Required]
        //public string Vessel { get; set; }
        //[Required]
        //public string Voyage { get; set; }
        //[Required]
        //public string Origin { get; set; }
        //[Required(ErrorMessage = "The Place of Carrier Receipt field is required")]
        //public string PlaceofCarrierReceipt { get; set; }
        //[Required]
        //public string ETD { get; set; }
        //[Required(ErrorMessage = "The Port Of Load field is required")]
        //public string PortOfLoad { get; set; }
        //[Required]
        //public string Destination { get; set; }
        //[Required(ErrorMessage = "The Place Of Carrrier Delivery field is required")]
        //public string PlaceOfCarrrierDelivery { get; set; }
        //[Required]
        //public string ETA { get; set; }
        //[Required(ErrorMessage = "The Port Of Discharge field is required")]
        //public string PortOfDischarge { get; set; }
        //[Required(ErrorMessage = "The Move Type field is required")]
        //public string MoveType { get; set; }
        //[Required(ErrorMessage = "The Shipment Type field is required")]
        //public string ShipmentType { get; set; }
        //[Required(ErrorMessage = "The Control TotalNumber Containers field is required")]
        //public string ControlTotalNumberContainers { get; set; }
        //[Required(ErrorMessage = "The Control TotalNumber Packages field is required")]
        //public string ControlTotalNumberPackages { get; set; }
        //[Required(ErrorMessage = "The Control TotalShipment Weight field is required")]
        //public string ControlTotalShipmentWeight { get; set; }
        [Required(ErrorMessage = "The Research Reason Code field is required")]
        public string ResearchReasonCode { get; set; }

        [Required]
        public string Status { get; set; }

        [Required(ErrorMessage = "The Revenue Analyst Auditor field is required")]
        public string RevenueAnalystAuditor { get; set; }

        [Required]
        //[AllowHtml]
        [DataType(DataType.Html)]
        public string Comments { get; set; }

        public DateTime LastModified { get; set; }

        [Required]
        public string LastModifiedBy { get; set; }

        //public int CustomerId { get; set; }
        //public string CustomerLocationCode { get; set; }
        [Computed]
        public string Company { get; set; }

        [Computed]
        public string UserName { get; set; }

        [Computed]
        public string LastModifiedAuditor { get; set; }
        [Computed]
        public string Shipper { get; set; }
    }
}
