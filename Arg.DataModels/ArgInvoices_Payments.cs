using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("[ArgInvoices.Payments]")]
    public class ArgInvoices_Payments
    {
        [Key]
        public int PaymentId { get; set; }
        public string Invoice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CompanyId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
        public string Region { get; set; }

        [Computed]
        public string Company { get; set; }
    }
}
