using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class InvoiceStatusDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.BalanceDue BalanceDueInfo { get; set; }

        public string CustomerName { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string InvoiceComment { get; set; }

        public SelectList InvoiceStatuses { get; set; }
    }
}
