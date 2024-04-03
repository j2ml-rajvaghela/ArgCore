using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class ResearchStatusDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.ResearchItems ResearchInfo { get; set; }

        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Statuses { get; set; }
    }
}
