using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("[BalanceDues.Scope]")]
    public class BalanceDues_Scope
    {
        [Key]
        public int ScopeID { get; set; }
        public int CompanyId { get; set; }
        public string Region { get; set; }
        public DateTime BeginningBalanceDueInvoiceDate { get; set; }
        public DateTime EndingBalanceDueInvoiceDate1 { get; set; }
        public decimal ScopeCount { get; set; }
        public decimal ScopeRevenue { get; set; }
        public string Comments { get; set; }
        public DateTime ScopeBeginDate { get; set; }
        public DateTime ScopeEndDate { get; set; }

       [Computed]
        public string ScopeDate
        {
            get
            {
                return ScopeBeginDate + "/" + ScopeEndDate;
            }
        }

        //[Ignore]
        public string ScopeDateTooltipLabel
        {
            get
            {
                return "Scope Date: " + ScopeDate;
            }
        }
    }
}

