using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("BalanceDues")]
    public class BalanceDue
    {
        [Dapper.Contrib.Extensions.Key]
        public int BalanceId { get; set; }

        [Required]
        public string Region { get; set; }

        [Required(ErrorMessage = "The Customer field is required")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        [Required]
        public string Bol { get; set; }

        [Required(ErrorMessage = "The Bol Execution Date field is required")]
        public DateTime BolExecutionDate { get; set; }

        [Required(ErrorMessage = "The Customer Location Code field is required")]
        public string CustomerLocationCode { get; set; }

        [Required(ErrorMessage = "The Booking field is required")]
        public string BookingId { get; set; }

        [Required(ErrorMessage = "The Balance Due Invoice field is required")]
        public string BalanceDueInvoice { get; set; }

        [Required(ErrorMessage = "The Balance Due Invoice Date field is required")]
        public DateTime? BalanceDueInvoiceDate { get; set; }

        [Required(ErrorMessage = "The Invoice Status field is required")]
        public string InvoiceStatus { get; set; }

        [Required(ErrorMessage = "The Collection Status field is required")]
        public string CollectionStatus { get; set; }

        [Required(ErrorMessage = "The Client Gl Status field is required")]
        public string ClientGlStatus { get; set; }

        [Required(ErrorMessage = "The Port Of Loading field is required")]
        public string PortOfLoading { get; set; }

        [Required(ErrorMessage = "The Port Of Discharge field is required")]
        public string PortOfDischarge { get; set; }

        [Required(ErrorMessage = "The Move Type field is required")]
        public string MoveType { get; set; }

        [Required(ErrorMessage = "The Balance Due Amount field is required")]
        public decimal BalanceDueAmount { get; set; }

        [Required(ErrorMessage = "The BD Error Code field is required")]
        public string BDErrorCode { get; set; }

        [Required(ErrorMessage = "The BD Description field is required")]
        public string BDDescription { get; set; }

        [Required(ErrorMessage = "The Close Reason Code field is required")]
        public string CloseReasonCode { get; set; }

        [Required]
        public string Comments { get; set; }

        [Required(ErrorMessage = "The Revenue Analyst Auditor field is required")]
        public string RevenueAnalystAuditor { get; set; }

        [Required(ErrorMessage = "The Revenue Analyst Collector field is required")]
        public string RevenueAnalystCollector { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }
        public string Currency { get; set; }
        public string ShippersRefNumber { get; set; }
        public string ConsigneeRefNumber { get; set; }
        public string PayorRefNumber { get; set; }

        [Required]
        public string Vessel { get; set; }

        [Required]
        public string Voyage { get; set; }

        [Required]
        public string Quote { get; set; }

        [Required(ErrorMessage = "The Invoice Type field is required")]
        public string InvoiceType { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string Customer { get; set; }

        [Computed]
        public string TotalBol { get; set; }

        [Computed]
        public string UserName { get; set; }

        [Computed]
        public string ErrorCodeDescription { get; set; }

        [Computed]
        public decimal PaymentAmount { get; set; }

        [Computed]
        public decimal AmountPaid { get; set; }

        //Used in Invoice PDF
        [Computed]
        public decimal BDAmountDue
        { get { return AmountDue - PaymentAmount; } }

        //[Ignore]
        public decimal AmountPaidFormatted
        { get { return AmountPaid; } }

        [Computed]
        public decimal ItemsAmount { get; set; }

        [Computed]
        public decimal OtherChargesAmount { get; set; }

        //[Ignore]
        public decimal TotalCharges
        { get { return ItemsAmount + OtherChargesAmount; } }

        //[Ignore]
        public decimal TotalChargesFormatted
        { get { return TotalCharges; } }

        //[Ignore]
        public decimal AmountDue
        { get { return TotalCharges - AmountPaid; } }

        //[Ignore]
        public decimal AmountDueFormatted
        { get { return AmountDue; } }

        [Computed]
        public string TotalCount { get; set; }

        [Computed]
        public DateTime ActualDepartureDate { get; set; }

        [Computed]
        public string PaymentCurrency { get; set; }
        [Computed]
        public decimal UnderBillings { get; set; }

        //[Ignore]
        public decimal UnderBillingsFormatted
        { get { return UnderBillings; } }

        //[Ignore]
        public string BalanceDueInvoiceFormatted
        { get { return String.Format("{0:n}", BalanceDueInvoice); } }

        [Computed]
        public int BolCount { get; set; }

        [Computed]
        public string RevenueAnalystAuditorName { get; set; }

        [Computed]
        public string RevenueAnalystCollectorName { get; set; }

        [Computed]
        public int BdCustomerId { get; set; }

        [Computed]
        public decimal BalanceDueAmountPaid { get; set; }

        [Computed]
        public string BalanceDueAmountWithCurrency { get; set; }

        [Computed]
        public decimal TotalBDAmount { get; set; }

        [Computed]
        public decimal TotalPaymentAmount { get; set; }

        [Computed]
        public string FullName { get; set; }

        [Computed]
        public string OriginalInv { get; set; }

        [Computed]
        public string Invoice { get; set; }

        [Computed]
        public DateTime InvoiceDate { get; set; }

        [Computed]
        public decimal InvoiceAmountDue { get; set; }
    }
}
