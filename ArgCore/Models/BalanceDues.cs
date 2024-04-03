using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class BalanceDues
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }
        public Arg.Ceva.DataAccess.SearchOptions SearchOptionsCeva { get; set; }

        public List<BalanceDue> BalanceDuesList { get; set; }

        public BalanceDue BalanceDuesDetail { get; set; }

        public SelectList Customers { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Regions { get; set; }

        public int CustomerCount { get; set; }

        public string TotalBD { get; set; }

        public string TotalBol { get; set; }

        public SelectList BDErrorCodes { get; set; }

        public SelectList InvoiceTypes { get; set; }

        public SelectList MoveTypes { get; set; }

        public SelectList Vessels { get; set; }

        public SelectList Voyages { get; set; }

        public SelectList InvoiceStatus { get; set; }

        public SelectList CollectionStatus { get; set; }

        public SelectList RevenueAnalystAuditors { get; set; }

        public SelectList RevenueAnalystCollectors { get; set; }

        public SelectList OriginLocationCodes { get; set; }

        public SelectList DestinationLocationCodes { get; set; }

        public SelectList POL { get; set; }

        public SelectList POD { get; set; }

        public SelectList ClientGlStatus { get; set; }

        public SelectList InvoiceNo { get; set; }

        public int CompanyId { get; set; }

        public BalanceDue ResultsByInvoiceStatus { get; set; }

        public decimal BOLCount { get; set; }

        public string TotalBDPaidAmount { get; set; }

        public int TotalBDCount { get; set; }

        public SelectList EmailTemplates { get; set; }

        public SelectList SMTPUsers { get; set; }

        public string Message { get; set; }

        public SelectList BDOtherChargeCodes { get; set; }

        public List<BalanceDue> BalanceDuesByStatus { get; set; }

        public SelectList CloseReasonCodes { get; set; }
    }
}
