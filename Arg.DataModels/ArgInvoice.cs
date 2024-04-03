using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("ArgInvoices")]
    public class ArgInvoice
    {
        [Dapper.Contrib.Extensions.Key]
        public int InvoiceId { get; set; }

        [Required]
        public string Invoice { get; set; }

        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The Invoice Type field is required")]
        public string InvoiceType { get; set; }

        //public int CustomerId { get; set; }
        [Required]
        public string Region { get; set; }

        //[Required]
        //public string Bol { get; set; }
        //[Required(ErrorMessage = "The Amount Due field is required")]
        //public string Amount_Due { get; set; }
        [Required(ErrorMessage = "The Invoice Status field is required")]
        public string InvoiceStatus { get; set; }

        public DateTime DueDate { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string Customer { get; set; }

        [Computed]
        public decimal AmountPaid { get; set; }

        [Computed]
        public decimal CommissionsAmountDueUSD { get; set; }

        [Computed]
        public decimal InvoiceAmountLocalCurrencySum { get; set; }

        [Computed]
        public decimal InvoiceAmountUSD { get; set; }

        [Computed]
        public decimal InvoiceAmountLocalCurrency { get; set; }

        [Computed]
        public string InvoiceTextField { get; set; }

        //[Ignore]
        public decimal NetDueARG
        { get { return InvoiceAmountUSD - CommissionsAmountDueUSD; } }

        [Computed]
        public string Address1 { get; set; }

        [Computed]
        public string Address2 { get; set; }

        [Computed]
        public string City { get; set; }

        [Computed]
        public string State { get; set; }

        [Computed]
        public string ZipCode { get; set; }
    }
}
