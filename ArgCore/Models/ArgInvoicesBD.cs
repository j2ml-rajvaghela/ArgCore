using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class ArgInvoicesBD
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }

        public List<ArgInvoice> ArgInvoicesList { get; set; }

        public ArgInvoices_BalanceDues ArgInvoicesBDDetail { get; set; }

        public  SelectList Customers { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Regions { get; set; }

        public string Message { get; set; }

        public int CompanyId { get; set; }
        public List<string> CompanyIds { get; set; }

        public SelectList InvoiceNo { get; set; }

        public string TotalInvoices { get; set; }

        public string TotalPayments { get; set; }

        public string TotalCommissions { get; set; }

        public string AmountOpen { get; set; }

        public SelectList Status { get; set; }
        public SelectList InvoiceTypes { get; set; }
        public SelectList InvoiceTypesPaid { get; set; }
    }
}
