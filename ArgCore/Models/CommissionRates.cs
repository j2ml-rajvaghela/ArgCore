using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class CommissionRates
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.CommissionRates> CommissionRatesList { get; set; }

        public SearchOptions SearchOptions { get; set; }

        public Arg.DataModels.CommissionRates CommissionRateDetail { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Regions { get; set; }

        public SelectList Customers { get; set; }

        public SelectList Users { get; set; }

        public IEnumerable<SelectListItem> InvoiceStatus { get; set; }

        public SelectList InvoiceNos { get; set; }

        public int CompanyId { get; set; }

        public string Message { get; set; }
    }
}
