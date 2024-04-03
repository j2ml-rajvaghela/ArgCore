using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class ActivityStats
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }

        public List<Arg.DataModels.ActivityStats> ActivityStatsList { get; set; }

        public SelectList Clients { get; set; }

        public SelectList WebPages { get; set; }

        public SelectList Users { get; set; }

        public SelectList IPAddresses { get; set; }

        public SelectList UserRoles { get; set; }

        public List<Arg.DataModels.ActivityStats> ActStatsByEvent { get; set; }
    }
}
