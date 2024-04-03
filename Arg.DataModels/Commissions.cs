using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("Commissions")]
    public class Commissions
    {
        [Key]
        public int CommissionId { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public string Region { get; set; }

        [Column("Invoice#")]
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerLocationCode { get; set; }
        public string BookingId { get; set; }

        [Column("Bol#")]
        public string BOLNo { get; set; }

        public DateTime BolExecutionDate { get; set; }
        public decimal AmountDueUSD { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string UserName { get; set; }

        [Computed]
        public string CustomerName { get; set; }

        [Computed]
        public string InvoiceStatus { get; set; }
    }
}
