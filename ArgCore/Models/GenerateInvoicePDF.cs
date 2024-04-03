namespace ArgCore.Models
{
    public class GenerateInvoicePDF
    {
        public List<Arg.DataModels.BalanceDues_Item> BDItems { get; set; }

        public List<Arg.DataModels.BalanceDues_OtherCharges> BDOtherCharges { get; set; }

        public List<Arg.DataModels.BalanceDue> AuditDetails { get; set; }

        public List<Arg.DataModels.BalanceDue> BalanceDueInfo { get; set; }

        public string BolNo { get; set; }

        public DateTime DueDate { get; set; }
        //public DateTime InvoiceDate { get; set; }
        //public string InvoiceDueDate { get; set; }
        //public string RemitTo { get; set; }
        //public string BillTo { get; set; }
        //public string InvoiceStatus { get; set; }
        //public string InvoiceNo { get; set; }

        public Arg.DataModels.ArgClient ClientDetails { get; set; }

        public Arg.DataModels.Customers CustomerDetails { get; set; }

        public Arg.DataModels.BalanceDues_Customers_Contacts CustomerContactDetails { get; set; }
    }
}
