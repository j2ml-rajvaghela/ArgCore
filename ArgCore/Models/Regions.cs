using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Regions
    {
        public int RegionId { get; set; }
        public int CompanyId { get; set; }

        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public List<Arg.DataModels.Regions> RegionsList { get; set; }
        public SelectList RegionsDropdownList { get; set; }

        public Arg.DataModels.Regions RegionDetail { get; set; }

        public SelectList Customers { get; set; }

        public SelectList Companies { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsGetRegion { get; set; }
    }
}
