using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class BDPaymentAmountDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.BalanceDue BalanceDueInfo { get; set; }

        [Required(ErrorMessage = "BDPaymentAmount field is required")]
        public decimal BDPaymentAmount { get; set; }

        public decimal TotalPaymentAmount { get; set; }

        public List<Arg.DataModels.BalanceDues_Payments> BDPayments { get; set; }

        public string PaymentType { get; set; }

        public string PaymentReference { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Currency { get; set; }

        public string Payor { get; set; }

        public SelectList Customers { get; set; }
        public Arg.DataModels.ArgInvoice InvoiceInfo { get; set; }
        public SelectList InvoiceStatuses { get; set; }
    }
}
