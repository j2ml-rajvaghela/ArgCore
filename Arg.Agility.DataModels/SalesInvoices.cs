using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("SalesInvoices")]
    public class SalesInvoices
    {
        public string BranchCode { get; set; }
        public string JobNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceTotalBase { get; set; }
        public string InvoiceCurrency { get; set; }
        public string ClientID { get; set; }
        public string ClientIDName { get; set; }
        public string InvoiceStatus { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceType { get; set; }
        public string InvUnitNumber { get; set; }
        public string ChargeCode { get; set; }
        public string ChargeDescription { get; set; }
        public string GroupChargeCode { get; set; }
        public string GroupChargeDescription { get; set; }
        public string OptionalComment { get; set; }
        public string ChargeValue { get; set; }
        public DateTime? loaded { get; set; }
        public string MD5 { get; set; }
        public string File_Name { get; set; }

        [Computed]
        public int ItemId { get; set; }

        [Computed]
        public string Description { get; set; }

        [Computed]
        public decimal AmountDue { get; set; }

        [Computed]
        public string InvoiceTotal
        {
            get
            {
                return string.IsNullOrEmpty(ChargeValue) ? "0.00" : Convert.ToDecimal(ChargeValue).ToString("F");
            }
        }
    }
}
