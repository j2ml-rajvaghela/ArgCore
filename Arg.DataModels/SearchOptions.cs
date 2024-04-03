using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    public class SearchOptions
    {
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public List<int> CompanyIds { get; set; }
        public List<string> CompIds { get; set; }

        //In Balance Dues table CustomerId is string
        public string CustomerId { get; set; }

        public int BDOtherChargeCodeId { get; set; }
        public List<string> CustomerIds { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public List<string> IpAddresses { get; set; }
        public string WebPage { get; set; }
        public List<string> WebPages { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Region { get; set; }
        public List<string> Regions { get; set; }
        public List<string> Clients { get; set; }

        public string RevenueAnalystAuditor { get; set; }
        public List<string> RevenueAnalystAuditors { get; set; }

        public List<string> BalanceDueInvoices { get; set; }

        public List<string> InvoiceStatus { get; set; }

        public string CloseReasonCode { get; set; }

        public List<string> CloseReasonCodes { get; set; }

        public List<string> CollectionStatus { get; set; }

        public List<string> ClientGlStatus { get; set; }

        public List<string> PortOfLoading { get; set; }

        public List<string> PortOfDischarge { get; set; }

        public DateTime BolExecutionStartDate { get; set; }
        public DateTime BolExecutionEndDate { get; set; }

        public DateTime BalanceDueInvoiceStartDate { get; set; }
        public DateTime BalanceDueInvoiceEndDate { get; set; }

        public DateTime BalanceDuePaymentStartDate { get; set; }
        public DateTime BalanceDuePaymentEndDate { get; set; }

        public string InvoiceStartDate { get; set; }
        public string InvoiceEndDate { get; set; }

        public DateTime LastModifiedStartDate { get; set; }
        public DateTime LastModifiedEndDate { get; set; }

        public DateTime DateAddedStart { get; set; }
        public DateTime DateAddedEnd { get; set; }

        public string Bol { get; set; }

        public string CustomerLocationCode { get; set; }

        public string BookingId { get; set; }

        public List<string> Vessel { get; set; }

        public List<string> Voyage { get; set; }

        public string Quote { get; set; }

        public string Tariff { get; set; }

        public List<string> MoveType { get; set; }

        public List<string> BDErrorCodes { get; set; }

        public string BDDescription { get; set; }

        public string InvoiceType { get; set; }
        public List<string> InvoiceTypes { get; set; }
        public List<string> InvoiceTypesPaid { get; set; }

        public string RevenueAnalystCollector { get; set; }
        public List<string> RevenueAnalystCollectors { get; set; }

        public int BalanceId { get; set; }

        public decimal BalanceDueAmount { get; set; }

        public int ResearchId { get; set; }

        public string Status { get; set; }
        public List<string> SelectedStatus { get; set; }

        public List<string> ResearchReasonCodes { get; set; }

        public string Email { get; set; }

        public string Contact { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public string CustomerName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        //Pasha DB
        //[DisplayFormat(DataFormatString = "mm-dd-yyyy")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public string DepartureStartDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "mm-dd-yyyy")]
        public string DepartureEndDate { get; set; }

        //public string DepartureEndDate { get; set; }

        public List<string> Mode { get; set; }

        public List<string> ShipperID { get; set; }

        public List<string> ConsigneeID { get; set; }

        public List<string> PayorID { get; set; }

        public string ShipperReferenceNumber { get; set; }

        public string ForwarderReferenceNumber { get; set; }

        public string ConsigneeReferenceNumber { get; set; }

        public string NotifyPartyReferenceNumber { get; set; }

        public string PayorReferenceNumber { get; set; }

        public decimal BookingMinWeight { get; set; }
        public decimal BookingMaxWeight { get; set; }

        public List<string> POL { get; set; }

        public List<string> POD { get; set; }

        public List<string> POLCode { get; set; }
        public List<string> PODCode { get; set; }

        [Display(Name = "Not Null Values")]
        public bool ShipperRefNoNotNullValues { get; set; }

        [Display(Name = "Not Null Values")]
        public bool ForwarderRefNoNotNullValues { get; set; }

        [Display(Name = "Not Null Values")]
        public bool ConsigneeRefNoNotNullValues { get; set; }

        [Display(Name = "Not Null Values")]
        public bool NotifyNoNotNullValues { get; set; }

        [Display(Name = "Not Null Values")]
        public bool PayorRefNoNotNullValues { get; set; }

        public List<string> EquipmentSize { get; set; }

        public List<string> EquipmentType { get; set; }

        public string CommodityCode { get; set; }

        public string CommodityDescription { get; set; }

        public decimal MinTemp { get; set; }

        public decimal MaxTemp { get; set; }

        public List<string> UNHazmatCode { get; set; }

        public bool SITFlag { get; set; }

        public string ReferenceType { get; set; }

        public string ReferenceValue { get; set; }

        public string FirstBolNO { get; set; }

        public string LastBolNO { get; set; }

        public List<string> OriginLocationCode { get; set; }
        public List<string> DestinationLocationCode { get; set; }

        public List<string> IssuingDepts { get; set; }
        public string ShipmentNo { get; set; }
        public string AWBBLNo { get; set; }
        public string ShipmentStartDate { get; set; }
        public string ShipmentEndDate { get; set; }
        public List<string> ShipmentType { get; set; }
        public List<string> ShipmentStatus { get; set; }
        public List<string> ShipmentCLStatus { get; set; }
        public List<string> Origin { get; set; }
        public List<string> Dest { get; set; }
        public decimal MinChargeableWeight { get; set; }
        public decimal MaxChargeableWeight { get; set; }
        public decimal? FLInput1 { get; set; }
        public decimal? FLInput2 { get; set; }
        public decimal? FLInput3 { get; set; }
        public decimal? FLInput4 { get; set; }
        public string FLSign1 { get; set; }
        public string FLSign2 { get; set; }
        public string FLSign3 { get; set; }
        public string FLSign4 { get; set; }


        public string BOLNo { get; set; }

        public string ChargeCode { get; set; }

        public string ChargeCodeOperator { get; set; }

        public string ChargeDesc { get; set; }

        public string ChargeDescOperator { get; set; }

        public string ChargeMinAmount { get; set; }
        public string ChargeMaxAmount { get; set; }

        public string TotalChargeMinAmount { get; set; }
        public string TotalChargeMaxAmount { get; set; }

        public decimal CurrentMinBalance { get; set; }
        public decimal CurrentMaxBalance { get; set; }

        public List<SortOptionsObj> SortOptions { get; set; }
        //[Display(Name = "Shipper Name")]
        //public bool SortByShipperName { get; set; }
        //public bool SortByShipperNameDesc { get; set; }

        //[Display(Name = "Origin Location Code")]
        //public bool SortByOrgLocCode { get; set; }
        //public bool SortByOrgLocCodeDesc { get; set; }

        //[Display(Name = "Dest. Location Code")]
        //public bool SortByDesLocCode { get; set; }
        //public bool SortByDesLocCodeDesc { get; set; }

        //[Display(Name = "Commodity Code")]
        //public bool SortByCommodityCode { get; set; }
        //public bool SortByCommodityCodeDesc { get; set; }

        //[Display(Name = "Mode")]
        //public bool SortByMode { get; set; }
        //public bool SortByModeDesc { get; set; }

        //[Display(Name = "Booking ID")]
        //public bool SortByBookingID { get; set; }
        //public bool SortByBookingIDDesc { get; set; }

        public string BDInvoiceStatus { get; set; }

        public int SMTPAccountId { get; set; }

        public int InvoiceTerms { get; set; }

        public string EmailTemplate { get; set; }

        public string SMTPUser { get; set; }

        public string ContainerId { get; set; }

        public string ImageType { get; set; }

        public bool EliminateBDResearchItems { get; set; }

        public string BOLViews { get; set; }

        //UserId
        public List<string> UserIds { get; set; }

        public List<string> BDOtherChargeCodes { get; set; }

        public int CommissionId { get; set; }

        public List<string> InvoiceNos { get; set; }
        public string InvoiceNo { get; set; }

        public bool AddBDToInvoice { get; set; }
        public bool DisplayDetails { get; set; }

        public string ClientStartDate { get; set; }
        public string ClientEndDate { get; set; }

        public List<string> ContainerEventType { get; set; }

        public string TrackingStartDate { get; set; }
        public string TrackingEndDate { get; set; }

        public List<string> Roles { get; set; }

        public string Message { get; set; }

        public string ClientDBName { get; set; }
        public bool ResultTableFormat { get; set; }
        public string TransactionViewStartDate { get; set; }
        public string TransactionViewEndDate { get; set; }
        public List<string> Analyst { get; set; }
        public string BillType { get; set; }
        public string ReportType { get; set; }
        public List<string> ReportTypes { get; set; }

        public string JobNumber { get; set; }
        public string ConsignmentID { get; set; }
        public string unitNumber { get; set; }

        public string JobConfirmationStartDate { get; set; }
        public string JobConfirmationEndDate { get; set; }
        public string LoadStartDate { get; set; }
        public string TempFromZone { get; set; }
        public string TempToZone { get; set; }
        public string LoadEndDate { get; set; }
        public List<string> ServiceMovementTypeCode { get; set; }
        public List<string> ServiceLevelCode { get; set; }
        public List<string> ServiceTypeCode { get; set; }
        public List<string> PortOfExitCode { get; set; }
        public List<string> PortofEntryCode { get; set; }
        public List<string> NotifyPartyCode { get; set; }
        public List<string> ExportingCarrierCode { get; set; }
        public List<string> UnitTypeCode { get; set; }
        public List<string> HazMatFlagCode { get; set; }
        public List<string> PrepaidCollectCode { get; set; }

        public static explicit operator SearchOptions(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class SortOptionsObj
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public int Idx { get; set; }
        public bool IsDesc { get; set; }
        public bool IsSelected { get; set; }
    }
}

