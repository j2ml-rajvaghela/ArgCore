using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("InvoiceSummary")]
    public class InvoiceSummary
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }

        public string PayorID { get; set; }
        public string InvoiceNumber { get; set; }
        public string PSNACT { get; set; }
        public decimal PrepaidAmount { get; set; }
        public decimal CollectAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal TotalCollectible { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string PSACYY { get; set; }
        public string PSACMM { get; set; }
        public string PSUSER { get; set; }
        public string PSDYMD { get; set; }
        public string PSTIME { get; set; }
        public string PSINVC { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string PSDIVN { get; set; }
        public string InvoiceType { get; set; }
        public string PSCRNO { get; set; }
        public string PSJDBN { get; set; }
        public string PSJDTM { get; set; }
        public string PSCTRM { get; set; }
        public string PSNCDE { get; set; }

        [Column("PSACY@")]
        public string PSACYNo { get; set; }

        [Column("PSINV@")]
        public string PSINVNo { get; set; }

        public DateTime? Uploaded { get; set; }

        [Computed]
        public string PrepaidCollectAmount { get; set; }

        [Computed]
        public string Payor { get; set; }

        [Computed]
        public decimal TotalCharges { get; set; }

        [Computed]
        public decimal TotalPayment { get; set; }

        [Computed]
        public decimal CurrentBalanceAmt
        { get { return TotalCharges - TotalPayment; } }
    }
}
