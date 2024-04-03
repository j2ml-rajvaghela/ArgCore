using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("CommissionRates")]
    public class CommissionRates
    {
        [Key]
        public int CommRateId { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public string Region { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Basis { get; set; }
        public string RateBasis { get; set; }
        public decimal Threshold { get; set; }
    }
}
