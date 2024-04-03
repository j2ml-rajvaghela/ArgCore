using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class ResearchItems
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }

        public List<Arg.DataModels.ResearchItems> ResearchItemsList { get; set; }

        public Arg.DataModels.ResearchItems ResearchItemDetail { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Customers { get; set; }

        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Status { get; set; }

        public SelectList Auditors { get; set; }

        public SelectList ReasonCodes { get; set; }

        public SelectList Regions { get; set; }

        public int CompanyId { get; set; }
    }
}
