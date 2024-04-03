using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class AnalystPerformance
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public SearchOptions SearchOptions { get; set; }
        public SelectList Shippers { get; set; }
        public SelectList Companies { get; set; }
        public SelectList Analyst { get; set; }
        public List<Arg.DataModels.ActivityStats> AnalystList { get; set; }
        public int CompanyId { get; set; }
    }
}
