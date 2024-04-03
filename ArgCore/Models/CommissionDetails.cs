using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class CommissionDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.Commissions> Commissions { get; set; }
        //public Arg.DataModels.Commissions Commission { get; set; }

        public List<CommissionSummery> CommissionsSummeryList { get; set; }
    }

    public class CommissionSummery
    {
        public string UserName { get; set; }
        public decimal AmountDueUSD { get; set; }
    }
}

