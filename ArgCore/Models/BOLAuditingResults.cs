using Arg.DataModels;
using ArgCore.Helpers;
using X.PagedList;

namespace ArgCore.Models
{
    public class BOLAuditingResults
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }
        public Arg.Agility.DataModels.BOLHeaders AgilityBOLAuditResults { get; set; }
        public Arg.Agility.DataModels.BookingHeaders AgilityBookingInfo { get; set; }

        //public Arg.Agility.DataModels.BookingHeaders AgilityBOLHeaderDetails { get; set; }
        public Arg.Agility.DataModels.ShipmentTrackingDetails AgilityShipmentTrackingDetails { get; set; }

        public List<Arg.Agility.DataModels.BOLContainerDetails> AgilityBOLContainerDetails { get; set; }
        public List<Arg.Agility.DataModels.SalesInvoices> AgilitySalesInvoices { get; set; }
        public List<Arg.Agility.DataModels.PurchaseInvoices> AgilityPurchaseInvoices { get; set; }
        public Arg.Agility.DataModels.DocumentImages AgilityDocumentImages { get; set; }

        public BOLHeader BOLAuditResults { get; set; }
        public Bookings BookingInfo { get; set; }

        public Bookings BookingItemDetails { get; set; }
        public Bookings BOLHeaderDetails { get; set; }
        public List<BOLCommodity> BOLCommodity { get; set; }

        //public Arg.DataModels.DocumentImages DocumentImageDetails { get; set; }
        public List<BOLChargesModel> BOLCharges { get; set; }

        public List<InvoiceSummary> InvoiceSummary { get; set; }
        public List<ContainerEventHistory> ContainerEventHistory { get; set; }
        public List<BookingsRemarks> BookingRemarks { get; set; }

        public List<BookingsNotes> BookingNotes { get; set; }

        //public IEnumerable<string> PDFFiles { get; set; }

        public List<BOLReference> BOLReferences { get; set; }

        public List<BOLRemarks> BOLRemarks { get; set; }

        public string FirstBolNo { get; set; }

        public string LastBolNo { get; set; }

        public string Message { get; set; }

        public int QueryId { get; set; }

        public int Idx { get; set; }

        public int TotalResultCount { get; set; }

        public List<DocumentImages> PDFFile { get; set; }
        public List<Arg.Agility.DataModels.DocumentImages> AgilityPDFFile { get; set; }

        public List<Arg.DataModels.ActivityStats> ActStatsByEvent { get; set; }

        public List<BalanceDue> BalanceDueDetails { get; set; }

        public bool ShowNavigation { get; set; }

        public List<BOLHeader> BolAuditResultStats { get; set; }
        public List<Arg.Agility.DataModels.BOLHeaders> AgilityBolAuditResultStats { get; set; }
        public IPagedList<BOLHeader> BolAuditResultTableFormat { get; set; }
        public IPagedList<Arg.Agility.DataModels.BOLHeaders> AgilityBolAuditResultTableFormat { get; set; }

        // public List<Arg.DataModels.BOLHeader> BolAuditResultTableFormat { get; set; }
        public BOLAuditSorting BOLAuditSorting { get; set; }
        public decimal BDPaymentAmount { get; set; }

        public string FileNames { get; set; }

        public bool EnablePullForResearchBtn { get; set; }

        public string BolNo { get; set; }
        public int CompanyId { get; set; }
        public string SpreedSheetUrl { get; set; }
        public string InvoiceBillType { get; set; }
        public ShipmentJournal ShipmentJournal { get; set; }
        public HouseHAWBAir HouseHAWBAirDetails { get; set; }
        public MasterAWB MasterAWBDetails { get; set; }
        public List<HellmannDocumentImages> HellmannDocuments { get; set; }
        public IPagedList<ShipmentJournal> ShipmentJournalAuditResultTableFormat { get; set; }
        public List<ShipmentJournal> ShipmentJournalAuditResultStats { get; set; }
    }
}
