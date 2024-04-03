using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class ClientManagementReports
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public SearchOptions SearchOptions { get; set; }
        public int CompanyId { get; set; }
        public SelectList Regions { get; set; }
        public SelectList Companies { get; set; }
        public IEnumerable<SelectListItem> ReportTypes { get; set; }
        public List<BalanceDues_Payments> RevenueRecovered { get; set; }
        public List<BalanceDues_Payments> OverchargeRevenueRecovered { get; set; }
        public List<BalanceDues_Payments> RevenueCollectedForPeriod { get; set; }
        public List<BalanceDues_Payments> RevenueCollectedForYTD { get; set; }
        public List<BalanceDues_Payments> RevenueCollectedForPastYear { get; set; }
        public decimal CurrentOpenBal { get; set; }
        public List<DateTime> BDInvDate { get; set; }
        public List<BalanceDues_Payments> RevLossByBDErrorCode { get; set; }
        public List<BalanceDues_Payments> OverchargeRevLossByBDErrorCode { get; set; }
        public List<BalanceDues_Payments> RevRecoveredByOrigin { get; set; }
        public List<BalanceDues_Payments> OverchargeRevRecoveredByOrigin { get; set; }
        public string CollectionRate { get; set; }
        public List<BalanceDues_Payments> RevenueLossTrend { get; set; }
        public List<BalanceDues_Payments> OverchargeRevenueLossTrend { get; set; }
        public string RevenueLossRate { get; set; }
        public string OverchargeRevenueLossRate { get; set; }
        public string Company { get; set; }
        public List<BalanceDues_Payments> RevByCustomer { get; set; }
        public List<BalanceDues_Payments> OverchargeRevByCustomer { get; set; }
        public List<BalanceDues_Scope> XAxisLabel { get; set; }
    }
}
