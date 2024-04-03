using Arg.Ceva.DataAccess;
using ArgCore.Helpers;
using X.PagedList;

namespace ArgCore.Models
{
    public class BookingAuditingResult
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.Ceva.DataAccess.SearchOptions SearchOptions { get; set; }

        public BookingHeader.BookingHeaderImp BOLAuditResults { get; set; }
        public List<BookingHeader_ContainerDetail.ContainerDetail> BOLContainerDetail { get; set; }
        public List<InvoiceCharges.InvoiceCharge> BOLInvoiceHeader { get; set; }
        public List<InvoiceCharges.InvoiceCharge> BOLNetInvoiceHeader { get; set; }
        public decimal TotalBOLInvoiceHeader { get; set; }
        public decimal NetBOLInvoiceHeader { get; set; }

        public Arg.DataModels.Bookings BookingInfo { get; set; }

        public Arg.DataModels.Bookings BookingItemDetails { get; set; }

        public Arg.DataModels.Bookings BOLHeaderDetails { get; set; }

        public List<Arg.DataModels.BOLCommodity> BOLCommodity { get; set; }

        //public Arg.DataModels.DocumentImages DocumentImageDetails { get; set; }

        public List<Arg.DataModels.BOLChargesModel> BOLCharges { get; set; }

        public List<Arg.DataModels.InvoiceSummary> InvoiceSummary { get; set; }

        public List<Arg.DataModels.ContainerEventHistory> ContainerEventHistory { get; set; }

        public List<Arg.DataModels.BookingsRemarks> BookingRemarks { get; set; }

        public List<Arg.DataModels.BookingsNotes> BookingNotes { get; set; }

        //public IEnumerable<string> PDFFiles { get; set; }

        public List<Arg.DataModels.BOLReference> BOLReferences { get; set; }

        public List<Arg.DataModels.BOLRemarks> BOLRemarks { get; set; }

        public string FirstBolNo { get; set; }

        public string LastBolNo { get; set; }

        public string Message { get; set; }

        public int QueryId { get; set; }

        public int Idx { get; set; }

        public int TotalResultCount { get; set; }

        public List<DocumentImages.DocumentImage> PDFFile { get; set; }

        public List<Arg.DataModels.ActivityStats> ActStatsByEvent { get; set; }

        public List<Arg.DataModels.BalanceDue> BalanceDueDetails { get; set; }

        public bool ShowNavigation { get; set; }

        public List<BookingHeader.BookingHeaderImp> BolAuditResultStats { get; set; }
        public IPagedList<BookingHeader.BookingHeaderImp> BolAuditResultTableFormat { get; set; }

        // public List<Arg.DataModels.BOLHeader> BolAuditResultTableFormat { get; set; }
        public decimal BDPaymentAmount { get; set; }

        public string FileNames { get; set; }

        public bool EnablePullForResearchBtn { get; set; }

        public string BolNo { get; set; }
        public int CompanyId { get; set; }
        public string SpreedSheetUrl { get; set; }
        public string InvoiceBillType { get; set; }
    }
}
