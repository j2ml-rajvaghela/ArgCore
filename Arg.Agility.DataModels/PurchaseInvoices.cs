using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("PurchaseInvoices")]
    public class PurchaseInvoices
    {
        public string BranchCode { get; set; }
        public string JobNumber { get; set; }
        public string ChargeCode { get; set; }
        public string ChargeCodeDescription { get; set; }
        public string InvoiceNumber { get; set; }
        public string SupplierID { get; set; }
        public string TransacttionType { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CurrencyCode { get; set; }
        public string BaseChargeValue { get; set; }
        public string BaseGoodsValue { get; set; }
        public string AllocatedValueBase { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceTotalBase { get; set; }
        public string PaymentTerms { get; set; }
        public DateTime? loaded { get; set; }
        public string MD5 { get; set; }
        public string File_Name { get; set; }
        public string InvoiceAmount { get; set; }

        public string InvoiceTotal
        {
            get
            {
                return string.IsNullOrEmpty(InvoiceAmount) ? "0.00" : Convert.ToDecimal(InvoiceAmount).ToString("F");
            }
        }
    }
}
