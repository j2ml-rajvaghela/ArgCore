using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("[BalanceDues.Payments]")]
    public class BalanceDues_Payments
    {
        [Key]
        public int PaymentId { get; set; }
        public int CompanyID { get; set; }
        public string Region { get; set; }
        public string CustomerID { get; set; }
        public string CustomerLocationCode { get; set; }
        public string BookingID { get; set; }

        [Column("BOL#")]
        public string BOLNo { get; set; }

        public DateTime BOLExecutionDate { get; set; }

        [Column("BalanceDueInvoice#")]
        public string BalanceDueInvoiceNo { get; set; }

        public DateTime BalanceDueInvoiceDate { get; set; }
        public string Payor { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string PaymentReference { get; set; }
        public string Currency { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string CustomerName { get; set; }

        [Computed]
        public decimal TotalPaymentAmount { get; set; }

        [Computed]
        public string RevenueRecovered { get; set; }

        [Computed]
        public string BDErrorCode { get; set; }

        [Computed]
        public decimal Percentage { get; set; }

        [Computed]
        public string RevenueRecoveredForPeriod { get; set; }

        [Computed]
        public string OverChargeRate { get; set; }

        [Computed]
        public string OverChargeCountForPeriod { get; set; }

        [Computed]
        public string CityState { get; set; }

        //[Ignore]
        public string RevenueRecoveredForPeriodFormatted

        {
            get
            {
                if (string.IsNullOrWhiteSpace(RevenueRecoveredForPeriod)) return "";
                if (RevenueRecoveredForPeriod.IndexOf(" ") <= 0) return RevenueRecoveredForPeriod;
                var values = RevenueRecoveredForPeriod.Split(' ');
                var decValue = Convert.ToDecimal(values[0]);
                string result = String.Format("{0:#,0.00}", decValue);
                return result + " " + values[1];
            }
        }

        [Computed]
        public string RevenueRecoveredForYTD { get; set; }

        //[Ignore]
        public string RevenueRecoveredForYTDFormatted
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RevenueRecoveredForYTD)) return "";
                if (RevenueRecoveredForYTD.IndexOf(" ") <= 0) return RevenueRecoveredForYTD;
                var values = RevenueRecoveredForYTD.Split(' ');
                var decValue = Convert.ToDecimal(values[0]);
                string result = String.Format("{0:#,0.00}", decValue);
                return result + " " + values[1];
            }
        }

        [Computed]
        public string RevenueRecoveredForPastYear { get; set; }

        //[Ignore]
        public string RevenueRecoveredForPastYearFormatted
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RevenueRecoveredForPastYear)) return "";
                if (RevenueRecoveredForPastYear.IndexOf(" ") <= 0) return RevenueRecoveredForPastYear;
                var values = RevenueRecoveredForPastYear.Split(' ');
                var decValue = Convert.ToDecimal(values[0]);
                string result = String.Format("{0:#,0.00}", decValue);
                return result + " " + values[1];
            }
        }

        [Computed]
        public string OriginLocationCode { get; set; }

        [Computed]
        public decimal BalanceDueAmount { get; set; }

        [Computed]
        public decimal PaymentTotal { get; set; }

        [Computed]
        public decimal ScopeRevenue { get; set; }

        [Computed]
        public string ShortDescription { get; set; }

        [Computed]
        public DateTime ScopeBeginDate { get; set; }

        [Computed]
        public DateTime ScopeEndDate { get; set; }

        [Computed]
        public string ScopeDate { get; set; }

        //public DateTime ScopeDate { get; set; }
        [Computed]
        public decimal LossRate { get; set; }

        [Computed]
        public decimal LossRateFormatted
        {
            get
            {
                return Math.Round(LossRate, 3);
            }
        }

        //[Ignore]
        //public string ScopeDate
        //{
        //    get
        //    {
        //        return ScopeBeginDate.ToString("MM/dd/yyyy") + " to " + ScopeEndDate.ToString("MM/dd/yyyy");
        //    }
        //}
        //[Ignore]
        public string ScopeDateTooltipLabel
        {
            get
            {
                return "Scope Date: " + ScopeDate;
            }
        }

        //[Ignore]
        public string PaymentAmountFormatted
        {
            get
            {
                return string.Format("{0:n}", PaymentAmount);
            }
        }

        //[Ignore]
        public decimal RevenueLossTrendValue
        {
            get
            {
                if (ScopeRevenue > 0)
                {
                    decimal value = (PaymentAmount / ScopeRevenue) * 100;
                    return Math.Round(value, 3);
                    //return String.Format("{0:#,0.00}", value);
                }
                return 0;
            }
        }

        //[Ignore]
        public decimal RevenueLossRatePercent
        {
            get
            {
                if (ScopeRevenue > 0)
                {
                    decimal value = (PaymentAmount / ScopeRevenue) * 100;
                    return value;
                }
                return 0;
            }
        }
    }
}
