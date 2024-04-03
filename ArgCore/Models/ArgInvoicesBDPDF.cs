namespace ArgCore.Models
{
    public class ArgInvoicesBDPDF
    {
        public List<Arg.DataModels.ArgInvoices_BalanceDues> BDInvoices { get; set; }

        public Arg.DataModels.ArgInvoice InvoiceInfo { get; set; }

        public decimal AmountDue { get; set; }

        public decimal AmountPaid { get; set; }

        public string Message { get; set; }
    }
}
