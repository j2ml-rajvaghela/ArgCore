using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Reports
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public SelectList SSISReports { get; set; }
        public SelectList Companies { get; set; }
        public string ReportID { get; set; }
        public int CompanyId { get; set; }
    }
}
