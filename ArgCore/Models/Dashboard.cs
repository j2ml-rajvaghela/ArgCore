using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class Dashboard
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.ActivityStats> ActivityStats { get; set; }

        public List<Arg.DataModels.BalanceDue> DailyBDSetUp { get; set; }

        public List<Arg.DataModels.BalanceDue> WeeklyBDSetUp { get; set; }

        public List<Arg.DataModels.BalanceDue> DailyBDCollected { get; set; }

        public List<Arg.DataModels.BalanceDue> WeeklyBDCollected { get; set; }

        public List<Arg.DataModels.BalanceDue> RevenueCollectedPastYear { get; set; }

        public List<Arg.DataModels.BalanceDue> OpenInvoices { get; set; }

        public List<Arg.DataModels.BalanceDue> RevenueCollectedForYearToDate { get; set; }

        public List<Arg.DataModels.BalanceDue> PendingBD { get; set; }

        public List<Arg.DataModels.BalanceDue> PendingApprovalBD { get; set; }

        public List<Arg.DataModels.ArgInvoices_BalanceDues> ClientOpenInvoices { get; set; }
    }
}
