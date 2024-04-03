using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Invoices
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.ArgInvoice> InvoicesList { get; set; }

        public Arg.DataModels.ArgInvoice InvoiceDetail { get; set; }

        public SelectList Customers { get; set; }
        public SelectList Clients { get; set; }

        public SelectList Companies { get; set; }
        public SelectList InvoiceTypes { get; set; }

        public IEnumerable<SelectListItem> InvoiceStatus { get; set; }

        public SelectList Regions { get; set; }
    }
}
