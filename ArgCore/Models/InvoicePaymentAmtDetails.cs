using ArgCore.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class InvoicePaymentAmtDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        [Required(ErrorMessage = "Invoice PaymentAmount field is required")]
        public decimal InvoicePaymentAmount { get; set; }

        public Arg.DataModels.ArgInvoice InvoiceInfo { get; set; }

        public List<Arg.DataModels.ArgInvoices_Payments> InvoicePayments { get; set; }

        public decimal TotalPaymentAmount { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentReference { get; set; }
    }
}
