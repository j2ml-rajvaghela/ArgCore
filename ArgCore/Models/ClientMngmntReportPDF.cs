namespace ArgCore.Models
{
    public class ClientMngmntReportPDF
    {
        public List<Arg.DataModels.BalanceDues_Payments> RevenueRecovered { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Company { get; set; }

        public decimal CurrentOpenBal { get; set; }

        public decimal CollectionRate { get; set; }

        public decimal RevenueLossRate { get; set; }

        public decimal RevenueLossTrend { get; set; }

        public List<DateTime> BDInvDate { get; set; }
    }
}
