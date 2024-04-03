using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Commissions
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.Commissions> CommissionsList { get; set; }

        public SearchOptions SearchOptions { get; set; }

        public Arg.DataModels.Commissions CommissionDetail { get; set; }

        public SelectList Companies { get; set; }
        public SelectList UserRoles { get; set; }

        public SelectList Regions { get; set; }

        public SelectList Customers { get; set; }

        public SelectList Users { get; set; }

        public IEnumerable<SelectListItem> InvoiceStatus { get; set; }

        public SelectList InvoiceNos { get; set; }

        public int CompanyId { get; set; }

        public string Message { get; set; }

        public string TotalCommissions { get; set; }

        public string TotalCommissionsPaid { get; set; }

        public string TotalAmountDue { get; set; }
    }
}
