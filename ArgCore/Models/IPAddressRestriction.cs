using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class IPAddressRestriction
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public SelectList Companies { get; set; }
        public List<Arg.DataModels.IPAddressRestriction> IPAddressRestrictionList { get; set; }
        public Arg.DataModels.IPAddressRestriction IPAddressRestrictionDetail { get; set; }
    }
}
