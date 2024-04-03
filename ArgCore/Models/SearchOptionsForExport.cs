namespace ArgCore.Models
{
    public class SearchOptionsForExport
    {
        public int CompanyId { get; set; }
        public string CustomerId { get; set; }
        public DateTime BalanceDueInvoiceStartDate { get; set; }
        public DateTime BalanceDueInvoiceEndDate { get; set; }
        public DateTime DateAddedStart { get; set; }
        public DateTime DateAddedEnd { get; set; }
        public string CollectionStatus { get; set; }
        public decimal BalanceDueAmount { get; set; }
        public string BalanceDueAmountSign { get; set; }
        public string CloseReasonCode { get; set; }
        public string Region { get; set; }
        public DateTime BolExecutionStartDate { get; set; }
        public DateTime BolExecutionEndDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string BDErrorCodes { get; set; }
        public string InvoiceType { get; set; }
    }
}
