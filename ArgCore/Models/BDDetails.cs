using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class BDDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.BalanceDue BalanceDueInfo { get; set; }

        public List<Arg.DataModels.BalanceDues_Item> BDItems { get; set; }

        public List<Arg.DataModels.BalanceDues_OtherCharges> BDOtherCharges { get; set; }

        public string BdErrorCodeDesc { get; set; }

        public System.Text.StringBuilder BDDesc { get; set; }

        public SelectList RevenueAnalystAuditors { get; set; }

        public SelectList RevenueAnalystCollectors { get; set; }
    }
}
